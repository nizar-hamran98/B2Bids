using Identity.Application.Commands;
using Identity.Application.Queries;
using Identity.Domain.Mapping;
using Identity.Domain.Models;
using MediatorCoordinator.Contract;
using MediatR;
using SharedKernel;

public class CreateUserPermissionRequest : IRequestContext<Result<UserPermissionModel>>
{
    public required UserPermissionModel Model { get; set; }
}

public class PostUserPermissionRequestHandler(Dispatcher _dispatcher) : IRequestHandler<CreateUserPermissionRequest, Result<UserPermissionModel>>
{
    public async Task<Result<UserPermissionModel>> Handle(CreateUserPermissionRequest request, CancellationToken cancellationToken)
    {
        var userPermissionResult = await _dispatcher.DispatchAsync(new GetUserPermissionQuery { UserId = request.Model.UserId, PermissionId = request.Model.PermissionId });
        if (userPermissionResult != null)
            return Result.Failure("User permission Already Exists");

        var entity = request.Model.ToEntity();
        await _dispatcher.DispatchAsync(new AddUpdateUserPermissionCommand { UserPermissions = entity });

        var model = entity.ToModel();

        return Result.Success(model);
    }
}
