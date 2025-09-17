using Identity.Application.Commands;
using Identity.Application.Queries;
using MediatorCoordinator.Contract;
using MediatR;
using SharedKernel;

namespace Identity.Application.RequestHandler;

public class ResetNumberOfRetriesRequest : IRequestContext<Result>
{
    public long UserId { get; set; }
}
public class ResetNumberOfRetriesRequestHandler(Dispatcher dispatcher) : IRequestHandler<ResetNumberOfRetriesRequest, Result>
{
    private readonly Dispatcher _dispatcher;

    public async Task<Result> Handle(ResetNumberOfRetriesRequest request, CancellationToken cancellationToken)
    {
        var user = await dispatcher.DispatchAsync(new GetUserQuery { Id = request.UserId });
        if (user == null)
            return Result.NotFound("User not found");

        user.AccessFailedCount = 0;
        await dispatcher.DispatchAsync(new AddUpdateUserCommand { User = user });

        return Result.Success("Number of Tries Has been Reset Successfully");
    }
}