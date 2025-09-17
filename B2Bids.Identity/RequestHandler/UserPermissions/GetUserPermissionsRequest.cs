using Identity.Application.Queries;
using Identity.Domain.DTOs;
using Identity.Domain.Mapping;
using MediatR;
using SharedKernel;

namespace Identity.Application.RequestHandler;
public class GetUserPermissionsRequest : IRequest<IEnumerable<UserPermissionDTO>>
{
    public long UserId { get; set; }
}

internal sealed class GetUserPermissionsRequestHandler(Dispatcher dispatcher) : IRequestHandler<GetUserPermissionsRequest, IEnumerable<UserPermissionDTO>>
{
    public async Task<IEnumerable<UserPermissionDTO>> Handle(GetUserPermissionsRequest request, CancellationToken cancellationToken)
    {
        if (request.UserId > 0)
        {
            var userPermissionResult = await dispatcher.DispatchAsync(new GetUserPermissionsByUserIdQuery() { UserId = request.UserId, AsNoTracking = true });
            return userPermissionResult.ToDtos();
        }
        else
        {
            var userPermissionsResult = await dispatcher.DispatchAsync(new GetUserPermissionsQuery());
            return userPermissionsResult.ToDtos();
        }
    }
}
