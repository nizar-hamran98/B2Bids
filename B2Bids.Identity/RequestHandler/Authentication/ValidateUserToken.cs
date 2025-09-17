using Identity.Application.Queries;
using Kernel.Contract;
using MediatorCoordinator.Contract;
using MediatR;
using SharedKernel;

namespace Identity.Application.RequestHandler;

public sealed record ValidateUserToken(string token) : IRequestContext<Result>;
internal class ValidateUserTokenHandler(Dispatcher dispatcher, IJwtManager jwtManager) : IRequestHandler<ValidateUserToken, Result>
{
    public async Task<Result> Handle(ValidateUserToken request, CancellationToken cancellationToken)
    {
        string userId = jwtManager.ParseTokenV2(request.token).FirstOrDefault().Value;

        var user = await dispatcher.DispatchAsync(new GetUserQuery { Id = long.Parse(userId), AsNoTracking = true });

        if (user == null)
            return Result.Failure("User Does Not Exist OR Token is Invalid!");


        return Result.Success("Token is Valid");
    }
}
