using Identity.Application.Commands;
using Identity.Application.Queries;
using Identity.Domain.DTOs;
using Identity.Domain.Entities;
using Identity.Domain.Mapping;
using Kernel.Contract;
using Kernel.Helpers;
using MediatorCoordinator.Contract;
using MediatR;
using Microsoft.AspNetCore.Http;
using SharedKernel;
using System.Security.Claims;
using Identity.Domain.Constants;

namespace Identity.Application.RequestHandler;

public class AuthenticateUserWithAzure : IRequestContext<Result<UserDTO>>
{
    public required ClaimsPrincipal User { get; set; }
    public required HttpRequest Request { get; set; }
}

public class AuthenticateUserWithAzureHandler(Dispatcher dispatcher, IIdentitySettings identitySettings, IJwtManager jwtManager) : IRequestHandler<AuthenticateUserWithAzure, Result<UserDTO>>
{
    public async Task<Result<UserDTO>> Handle(AuthenticateUserWithAzure data, CancellationToken cancellationToken)
    {
        var email = data.User.FindFirst(ClaimsConstants.preferred_username)?.Value;
        var userInfo = await dispatcher.DispatchAsync(new GetUserQuery { Email = email }, cancellationToken);
        if (userInfo != null)
        {
            return await GenerateSession(userInfo, data.Request);
        }
        else
        {
            return await CreateUser(email!, data.User, data.Request);
        }
    }

    private async Task<Result<UserDTO>> GenerateSession(User user, HttpRequest request)
    {
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

    private async Task<Result<UserDTO>> CreateUser(string email, ClaimsPrincipal user, HttpRequest request)
    {
        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var fullName = user.FindFirst("name")?.Value;

        var newUser = new User
        {
            UserName = fullName ?? email!,
            NormalizedUserName = (fullName ?? email!).ToUpper(),
            Email = email ?? string.Empty,
            NormalizedEmail = email?.ToUpper() ?? string.Empty,
            FullName = fullName ?? string.Empty,
            AccessFailedCount = 0,
            CreatedAt = DateTime.UtcNow,
            IsAllowAccess = true,
            LockoutEnabled = true,
            PasswordHash = HashPassword.Hash(new Random().Next(10000000, 99999999).ToString()),
            RoleId = 1,
            ExternalCode = userId,
            PhoneNumber = "+966666666",
            StatusId = (short)EntityStatus.Active
        };

        await dispatcher.DispatchAsync(new AddUpdateUserCommand { User = newUser });

        var userInfo = await dispatcher.DispatchAsync(new GetUserQuery { Email = email });

        return await GenerateSession(userInfo, request);
    }
}
