using Identity.Application.Queries;
using Identity.Domain.Mapping;
using Identity.Domain.Models;
using MediatR;
using SharedKernel;

namespace Identity.Application.RequestHandler;

public class GetPermissionRequest : IRequest<Result<PermissionModel>>
{
    public int PermissionId { get; set; }
}

public class GetPermissionRequestHandler(Dispatcher _dispatcher) : IRequestHandler<GetPermissionRequest, Result<PermissionModel>>
{
    public async Task<Result<PermissionModel>> Handle(GetPermissionRequest request, CancellationToken cancellationToken)
    {
        var permission = await _dispatcher.DispatchAsync(new GetPermissionQuery { Id = request.PermissionId, AsNoTracking = true });
        var permissionModel = permission.ToModel();

        return Result.Success(permissionModel);
    }
}
