using Identity.Application.Commands;
using Identity.Application.Queries;
using MediatorCoordinator.Contract;
using MediatR;
using SharedKernel;

namespace Identity.Application.RequestHandler;

public class DeleteUserRequest : IRequestContext<Result<bool>>
{
    public int Id { get; set; }
}

public class DeleteUserRequestHandler(Dispatcher dispatcher) : IRequestHandler<DeleteUserRequest, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeleteUserRequest request, CancellationToken cancellationToken)
    {
        var user = await dispatcher.DispatchAsync(new GetUserQuery { Id = request.Id }, cancellationToken);
        if (user == null)
        {
            return Result.NotFound("User not found");
        }


        await dispatcher.DispatchAsync(new DeleteUserCommand { User = user }, cancellationToken);
        return Result.Success("Successfully Deleted");
    }
}