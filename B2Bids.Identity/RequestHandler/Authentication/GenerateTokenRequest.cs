
using Identity.Application.Commands;
using Identity.Application.Queries;
using Identity.Domain.DTOs;
using Identity.Domain.Entities;
using Identity.Domain.Mapping;
using Identity.Domain.Models;
using Kernel.Contract;
using MediatorCoordinator.Contract;
using MediatR;
using Microsoft.AspNetCore.Http;
using SharedKernel;

namespace Identity.Application.RequestHandler;

public sealed record GenerateTokenRequest(AuthenticationModel Model, HttpRequest Request) : IRequestContext<UserDTO>;

public class GenerateTokenRequestHandler(Dispatcher _dispatcher,
        IJwtManager _jwtManager, IIdentitySettings _identitySettings) : IRequestHandler<GenerateTokenRequest, UserDTO>
{

    public async Task<UserDTO> Handle(GenerateTokenRequest data, CancellationToken cancellationToken)
    {


        User user = await _dispatcher.DispatchAsync(new AuthenticateQuery { entity = data.Model, AsNoTracking = true }, cancellationToken);

        return await GenerateSession(user!, data.Request);
    }
    private async Task<UserDTO> GenerateSession(User user, HttpRequest request)
    {
        var userObj = user.ToModel();
        var userLogin = await _dispatcher.DispatchAsync(new UserLoginQuery { UserId = userObj.Id });

        Dictionary<string, object> tokenobj = (Dictionary<string, object>)_jwtManager
            .GenerateEntityToken(userObj, _identitySettings.UserLoginSetting.TokenExpiryInSeconds);

        if (userLogin is null)
        {
            userLogin = UserLoginExtension
             .CreateUISession(tokenobj["access_token"].ToString(), userObj.Id,
             request.HttpContext.Connection.RemoteIpAddress.ToString(),
             request.HttpContext.Connection.RemoteIpAddress.ToString(),
            request.Headers["User-Agent"],
             _identitySettings.UserLoginSetting.TokenRefreshExpiryInMinutes);
        }
        else
        {
            UserLoginExtension
               .ReLoginSession(userLogin,
               tokenobj["access_token"].ToString() ?? string.Empty,
               request.HttpContext.Connection.RemoteIpAddress.ToString(),
               _identitySettings.UserLoginSetting.TokenRefreshExpiryInMinutes);
        }

        await _dispatcher.DispatchAsync(new AddUpdateUserLoginCommand { UserLogin = userLogin });
        tokenobj.Add("refresh_token", userLogin.RefreshTokenId);

        UserDTO userDto = user.ToDto();

        userDto.token = tokenobj;

        return userDto;
    }
}