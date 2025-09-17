using Identity.Application.Queries;
using Identity.Domain.Mapping;
using Identity.Domain.Models;
using MediatR;
using SharedKernel;

namespace Identity.Application.RequestHandler;

public class GetRolesRequest : IRequest<IEnumerable<RoleModel>>
{
    public EntityStatus Status { get; set; }
}

internal sealed class GetRolesRequestHandler(Dispatcher dispatcher) : IRequestHandler<GetRolesRequest, IEnumerable<RoleModel>>
{
    public async Task<IEnumerable<RoleModel>> Handle(GetRolesRequest request, CancellationToken cancellationToken)
    {
        if (request.Status == EntityStatus.Active)
        {
            var rolesResult = await dispatcher.DispatchAsync(new GetRolesQuery() { StatusId = (short)EntityStatus.Active });
            return rolesResult.ToModels();
        }
        else
        {
            var rolesResult = await dispatcher.DispatchAsync(new GetRolesQuery());
            return rolesResult.ToModels();
        }
    }
}
