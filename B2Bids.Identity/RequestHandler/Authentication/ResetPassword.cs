using Identity.Application.Queries;
using Identity.Domain.Entities;
using Identity.Domain.Mapping;
using Kernel.Contract;
using MediatorCoordinator.Contract;
using MediatR;
using SharedKernel;

namespace Identity.Application.RequestHandler;

public sealed record ResetPassword(string Email) : IRequestContext<Result>;
internal class ResetPasswordHandler(Dispatcher dispatcher, IJwtManager jwtManager) : IRequestHandler<ResetPassword, Result>
{
    public async Task<Result> Handle(ResetPassword data, CancellationToken cancellationToken)
    {
        User user = await dispatcher.DispatchAsync(new GetUserQuery { Email = data.Email, AsNoTracking = true }, cancellationToken);
        Domain.Models.UserModel userObj = user.ToModel();
        if (userObj == null)
            return Result.Failure("Email doesn't Exist");

        jwtManager.GenerateToken(userObj.Id.ToString(), 6000);

        return Result.Success();
    }
}