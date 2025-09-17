using Identity.Application.Queries;
using Identity.Domain.DTOs;
using Identity.Domain.Mapping;
using Identity.Domain.Models;
using MediatR;
using SharedKernel;

namespace Identity.Application.RequestHandler;
public class GetUserPermissionRequest : IRequest<UserPermissionDTO>
{
    public long Id { get; set; }
}

internal sealed class GetUserPermissionRequestHandler(Dispatcher dispatcher) : IRequestHandler<GetUserPermissionRequest, UserPermissionDTO>
{
    public async Task<UserPermissionDTO> Handle(GetUserPermissionRequest request, CancellationToken cancellationToken)
    {
        var UserPermissionResult = await dispatcher.DispatchAsync(new GetUserPermissionQuery { Id = request.Id , AsNoTracking = true });
        return UserPermissionResult.ToDto();
    }
}
