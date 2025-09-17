using Identity.Application.Commands;
using Identity.Application.Queries;
using Identity.Domain.DTOs;
using Identity.Domain.Entities;
using Identity.Domain.Mapping;
using Identity.Domain.Models;
using FluentValidation;
using Kernel.Contract;
using MediatorCoordinator.Contract;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SharedKernel;
using static Identity.Domain.AppSettingsBase;

namespace Identity.Application.RequestHandler;

public sealed record AuthenticateUser(AuthenticationModel Model, HttpRequest Request, bool CheckExternalCode, bool CheckUserIsAllowAccess) : IRequestContext<Result<UserDTO>>;
internal class AuthenticateUserHandler(
    IValidator<User> validatorUser,
    Dispatcher dispatcher,
    IJwtManager jwtManager,
    IIdentitySettings identitySettings, IServiceProvider serviceProvider) : IRequestHandler<AuthenticateUser, Result<UserDTO>>
{
    private readonly short? numberOfRetry = serviceProvider.GetRequiredService<IOptions<Authentication>>().Value.NumberOfLoginRetry;

    public async Task<Result<UserDTO>> Handle(AuthenticateUser data, CancellationToken cancellationToken)
    {

        var user = await dispatcher.DispatchAsync(new AuthenticateQuery { entity = data.Model }, cancellationToken);

        if (user == null)
        {
            var userInfo = await dispatcher.DispatchAsync(new GetUserQuery { UserName = data.Model.UserName, Email = data.Model.Email }, cancellationToken);
            return await UpdateTriesAndThrow(userInfo);
        }

        validatorUser.Validate(user!);

        return !user!.IsAllowAccess && data.CheckUserIsAllowAccess
            ? (Result<UserDTO>)Result.Failure("User Not Allow to Access")
            : await GenerateSession(user!, data.Request);
    }

    private async Task<Result> UpdateTriesAndThrow(User? user)
    {
        string Exception = "Invalid Password";

        if (user == null)
        {
            Exception = "User doesn't exist";
        }
        else if (numberOfRetry.HasValue)
        {
            Exception = "your Account has been Locked out!, please contact your Administrator";
            if (user.AccessFailedCount < numberOfRetry)
            {
                user.AccessFailedCount++;
                await dispatcher.DispatchAsync(new AddUpdateUserCommand { User = user });
                Exception = $"Invalid Password, you have {numberOfRetry - user.AccessFailedCount} tries remaining!";
            }
        }
        return Result.Success(Exception);
    }
    private async Task SetAccessFailedCountToZero(User user)
    {
        if (user.AccessFailedCount > 0)
        {
            user.AccessFailedCount = 0;
            await dispatcher.DispatchAsync(new AddUpdateUserCommand { User = user });
        }
    }
    private async Task<Result<UserDTO>> GenerateSession(User user, HttpRequest request)
    {
        if (user.AccessFailedCount >= numberOfRetry)
        {
            return Result.Failure("Max number of retry has been reached, please contact your Administrator");

        }
        else if (user.AccessFailedCount < numberOfRetry && user.AccessFailedCount != 0)
        {
            await SetAccessFailedCountToZero(user!);
        }

        var userObj = user.ToModel();
        var userLogin = await dispatcher.DispatchAsync(new UserLoginQuery { UserId = userObj.Id });

        Dictionary<string, object> tokenobj = (Dictionary<string, object>)jwtManager
            .GenerateEntityToken(userObj, identitySettings.UserLoginSetting.TokenExpiryInSeconds);

        if (userLogin is null)
        {
            userLogin = UserLoginExtension
             .CreateUISession(tokenobj["access_token"].ToString(), userObj.Id,
             request.HttpContext.Connection.RemoteIpAddress.ToString(),
             request.HttpContext.Connection.RemoteIpAddress.ToString(),
            request.Headers["User-Agent"],
             identitySettings.UserLoginSetting.TokenRefreshExpiryInMinutes);
        }
        else
        {
            UserLoginExtension
               .ReLoginSession(userLogin,
               tokenobj["access_token"].ToString() ?? string.Empty,
               request.HttpContext.Connection.RemoteIpAddress.ToString(),
               identitySettings.UserLoginSetting.TokenRefreshExpiryInMinutes);
        }

        await dispatcher.DispatchAsync(new AddUpdateUserLoginCommand { UserLogin = userLogin });
        tokenobj.Add("refresh_token", userLogin.RefreshTokenId);

        UserDTO userDto = user.ToDto();

        userDto.token = tokenobj;

        return Result.Success(userDto);
    }
}