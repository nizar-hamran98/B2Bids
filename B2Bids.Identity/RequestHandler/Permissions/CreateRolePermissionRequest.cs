using Identity.Application.Commands;
using Identity.Application.Queries;
using Identity.Domain.Entities;
using Identity.Domain.Mapping;
using Identity.Domain.Models;
using MediatorCoordinator.Contract;
using MediatR;
using SharedKernel;

namespace Identity.Application.RequestHandler;

public class CreateRolePermissionRequest : IRequestContext<Result<RolePermissionModel>>
{
    public required RolePermissionModel RolePermission { get; set; }
}

public class CreateRolePermissionRequestHandler(Dispatcher _dispatcher) : IRequestHandler<CreateRolePermissionRequest, Result<RolePermissionModel>>
{

    public async Task<Result<RolePermissionModel>> Handle(CreateRolePermissionRequest request, CancellationToken cancellationToken)
    {
        RolePermissions rolePermission = request.RolePermission.ToEntity();
        RolePermissions rolePermissionDt =
                       await _dispatcher.DispatchAsync(new GetRolePermissionQuery { PermissionId = request.RolePermission.PermissionId, RoleId = request.RolePermission.RoleId, AsNoTracking = true });

        if (rolePermission.StatusId == (short)EntityStatus.Active)
        {
            if (rolePermissionDt == null)
            {
                await _dispatcher.DispatchAsync(new AddUpdateRolePermissionCommand { RolePermission = rolePermission });
                request.RolePermission = rolePermission.ToModel();
            }
            else
            {
                rolePermissionDt.StatusId = (short)EntityStatus.Active;
                await _dispatcher.DispatchAsync(new AddUpdateRolePermissionCommand { RolePermission = rolePermissionDt });
                request.RolePermission = rolePermission.ToModel();
            }
        }
        else
        {
            await _dispatcher.DispatchAsync(new GetRolePermissionQuery { PermissionId = request.RolePermission.PermissionId, RoleId = request.RolePermission.RoleId, AsNoTracking = true });

            if (rolePermissionDt != null)
            {
                if (rolePermissionDt.StatusId == (short)EntityStatus.Active)
                {
                    rolePermissionDt.StatusId = (short)EntityStatus.Inactive;
                    await _dispatcher.DispatchAsync(new AddUpdateRolePermissionCommand { RolePermission = rolePermissionDt });
                    request.RolePermission = rolePermissionDt.ToModel();
                }
            }
        }
        return Result.Success(request.RolePermission);
    }
}
