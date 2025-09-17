using Identity.Application.Queries;
using Identity.Domain.Mapping;
using Identity.Domain.Models;
using MediatorCoordinator.Contract;
using MediatR;
using SharedKernel;

namespace Identity.Application.RequestHandler;

public class GetRolePermissionsRequest : IRequestContext<Result<IEnumerable<RolePermissionModel>>>
{
    public long RoleId { get; set; }
}

public class GetRolePermissionsRequestHandler(Dispatcher _dispatcher) : IRequestHandler<GetRolePermissionsRequest, Result<IEnumerable<RolePermissionModel>>>
{
    public async Task<Result<IEnumerable<RolePermissionModel>>> Handle(GetRolePermissionsRequest request, CancellationToken cancellationToken)
    {
        var servicePermission = await _dispatcher.DispatchAsync(new GetRolePermissionsQuery { RoleId = request.RoleId, IsActive = true, AsNoTracking = true });

        var PermissionModel = servicePermission.ToModels();

        if (!PermissionModel.Any())
            return Result.NotFound("No Permissions Found For This Role");


        return Result.Success(PermissionModel);
    }
}
