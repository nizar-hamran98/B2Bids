using Identity.Application.Commands;
using Identity.Application.Queries;
using Identity.Domain.Mapping;
using MediatorCoordinator.Contract;
using MediatR;
using SharedKernel;

namespace Identity.Application.RequestHandler;

public class DeleteRoleRequest : IRequestContext<Result<bool>>
{
    public long RoleId { get; set; }
}

public class DeleteRoleRequestHandler(Dispatcher dispatcher) : IRequestHandler<DeleteRoleRequest, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeleteRoleRequest request, CancellationToken cancellationToken)
    {
        var role = await dispatcher.DispatchAsync(new GetRoleQuery { Id = request.RoleId }, cancellationToken);
        if (role == null)
        {
            return Result.NotFound("Role not found");
        }


        await dispatcher.DispatchAsync(new DeleteRoleCommand { Role = role }, cancellationToken);
        return Result.Success("Successfully Deleted");
    }
}