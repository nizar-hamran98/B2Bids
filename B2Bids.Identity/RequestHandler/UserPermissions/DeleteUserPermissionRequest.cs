using Identity.Application.Commands;
using Identity.Application.Queries;
using Identity.Domain.Mapping;
using MediatorCoordinator.Contract;
using MediatR;
using SharedKernel;

namespace Identity.Application.RequestHandler;

public class DeleteUserPermissionRequest : IRequestContext<Result<bool>>
{
    public int UserPermissionId { get; set; }
}

public class DeleteUserPermissionRequestHandler(Dispatcher dispatcher) : IRequestHandler<DeleteUserPermissionRequest, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeleteUserPermissionRequest request, CancellationToken cancellationToken)
    {
        var userPermission = await dispatcher.DispatchAsync(new GetUserPermissionQuery { Id = request.UserPermissionId }, cancellationToken);
        if (userPermission == null)
            return Result.NotFound("UserPermission not found");

        var userPermissionDto = userPermission.ToDto();
        await dispatcher.DispatchAsync(new DeleteUserPermissionCommand { UserPermissions = userPermission }, cancellationToken);

        return Result.Success("Successfully Deleted");

    }
}