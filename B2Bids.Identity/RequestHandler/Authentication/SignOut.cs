using Identity.Application.Commands;
using Identity.Application.Queries;
using Identity.Domain.Entities;
using Kernel.Contract;
using MediatorCoordinator.Contract;
using MediatR;
using SharedKernel;
using System.Security.Claims;

namespace Identity.Application.RequestHandler;

public sealed record SignOut(string AccessToken, string RemoteIpAddress) : IRequestContext<Result>;
internal class SignOutHandler(Dispatcher dispatcher, IJwtManager jwtManager) : IRequestHandler<SignOut, Result>
{
    public async Task<Result> Handle(SignOut data, CancellationToken cancellationToken)
    {
        if (data.AccessToken.IsNullOrEmpty())
            return Result.Failure("Token is not provided in the headers");

        IEnumerable<Claim>? claims = jwtManager.ParseToken(data.AccessToken, false);
        Claim? UserId = claims.Filter(x => x.Type == "Id")?.FirstOrDefault();

        UserLogin? userLogin = await dispatcher.DispatchAsync(new UserLoginQuery { UserId = long.Parse(UserId.Value) }, cancellationToken);
        if (userLogin is null)
        {
            return Result.Failure();
        }

        UserLoginExtension.LogoutSession(userLogin, data.RemoteIpAddress);

        await dispatcher.DispatchAsync(new AddUpdateUserLoginCommand { UserLogin = userLogin }, cancellationToken);

        return Result.Success();
    }
}