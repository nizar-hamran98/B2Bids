using Identity.Application.Commands;
using Identity.Application.Queries;
using Identity.Domain.Entities;
using Identity.Domain.Mapping;
using Identity.Domain.Models;
using FluentValidation;
using Kernel.Contract;
using MediatorCoordinator.Contract;
using MediatR;
using SharedKernel;
using System.Security.Claims;

namespace Identity.Application.RequestHandler;

public sealed record RefreshToken(string AccessToken, string RefreshTokenId, string RemoteIpAddress) : IRequestContext<Result<Dictionary<string, object>>>;
internal class RefreshTokenHandler(Dispatcher dispatcher, IJwtManager jwtManager, IValidator<RefreshToken> validatorToken, IValidator<User> validatorUser, IIdentitySettings identitySettings) : IRequestHandler<RefreshToken, Result<Dictionary<string, object>>>
{
    public async Task<Result<Dictionary<string, object>>> Handle(RefreshToken data, CancellationToken cancellationToken)
    {
        validatorToken.Validate(data);

        IEnumerable<Claim>? claims = jwtManager.ParseToken(data.AccessToken, false);
        Claim? UserId = claims.Filter(x => x.Type == "Id")?.FirstOrDefault();

        UserLogin? UserLogin = await dispatcher
            .DispatchAsync(new UserLoginQuery { UserId = long.Parse(UserId.Value), RefreshToken = data.RefreshTokenId.ToString() },
            cancellationToken);

        Result? isAllowToRefreshToken = IsAllowToRefreshToken(UserLogin);
        return isAllowToRefreshToken is not null
            ? (Result<Dictionary<string, object>>)isAllowToRefreshToken
            : (Result<Dictionary<string, object>>)await GenerateSession(UserLogin, data.RemoteIpAddress, cancellationToken);
    }
    private Result? IsAllowToRefreshToken(UserLogin userLogin)
    {
        if (userLogin is null)
            return Result.Failure("User logged out, you need to re-login again");

        if (!userLogin.IsRefreshable || userLogin.RefreshTokenId.IsNullOrEmpty())
            return Result.Failure("User is not allow to refresh token");

        TimeSpan? difference = DateTime.UtcNow - userLogin.LastRefreshToken;

        return userLogin.ExpiryDate < DateTime.UtcNow && difference.HasValue
            && difference.Value.TotalMinutes > identitySettings.UserLoginSetting.DelayBeforeLoggingOutInMinutes
            ? Result.Failure("User logged out, you need to re-login again")
            : null;
    }
    private async Task<Dictionary<string, object>> GenerateSession(UserLogin userLogin, string RemoteIpAddress, CancellationToken cancellationToken)
    {
        User User = await dispatcher
     .DispatchAsync(new GetUserQuery
     { Id = userLogin.UserId, IncludeClaims = true, IncludeRole = true, IncludeRoles = true, IncludeUserRoles = true }, cancellationToken);

        validatorUser.Validate(User);

        UserModel userObj = User.ToModel();

        Dictionary<string, object> tokenobj = (Dictionary<string, object>)jwtManager
            .GenerateEntityToken(userObj, identitySettings.UserLoginSetting.TokenExpiryInSeconds);

        UserLoginExtension
            .RefreshSession(userLogin, tokenobj["access_token"].ToString() ?? "", RemoteIpAddress);

        await dispatcher.DispatchAsync(new AddUpdateUserLoginCommand { UserLogin = userLogin }, cancellationToken);

        tokenobj.Add("refresh_token", userLogin.RefreshTokenId);

        return tokenobj;
    }
}