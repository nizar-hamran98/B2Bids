using Identity.Application.Queries;
using Identity.Domain.Mapping;
using Identity.Domain.Models;
using MediatR;
using SharedKernel;

namespace Identity.Application.RequestHandler;

public class GetRoleRequest : IRequest<RoleModel>
{
    public long Id { get; set; }
}

internal sealed class GetRoleRequestHandler(Dispatcher dispatcher) : IRequestHandler<GetRoleRequest, RoleModel>
{
    public async Task<RoleModel> Handle(GetRoleRequest request, CancellationToken cancellationToken)
    {
        var roleResult = await dispatcher.DispatchAsync(new GetRoleQuery { Id = request.Id, AsNoTracking = true });

        var roleResultModel = roleResult.ToModel();
        return roleResultModel;

    }
}
