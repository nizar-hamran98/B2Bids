using Identity.Application.Queries;
using Identity.Domain.Mapping;
using Identity.Domain.Models;
using MediatR;
using SharedKernel;

namespace Identity.Application.RequestHandler;

public class GetPermissionsRequest : IRequest<Result<IEnumerable<PermissionModel>>>
{

}

public class GetPermissionsRequestHandler(Dispatcher _dispatcher) : IRequestHandler<GetPermissionsRequest, Result<IEnumerable<PermissionModel>>>
{


    public async Task<Result<IEnumerable<PermissionModel>>> Handle(GetPermissionsRequest request, CancellationToken cancellationToken)
    {
        var permissions = await _dispatcher.DispatchAsync(new GetPermissionsQuery() { AsNoTracking = true });

        var permissionsModel = permissions.ToModels();
        return Result.Success(permissionsModel);
    }
}