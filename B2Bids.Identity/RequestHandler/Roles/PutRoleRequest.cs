using Identity.Application.Commands;
using Identity.Application.Queries;
using Identity.Domain.Mapping;
using Identity.Domain.Models;
using LanguageExt;
using MediatorCoordinator.Contract;
using MediatR;
using SharedKernel;

public class PutRoleRequest : IRequestContext<Result<RoleModel>>
{
    public long RoleId { get; set; }
    public required RoleModel Model { get; set; }
}

public class PutRoleRequestHandler(Dispatcher dispatcher) : IRequestHandler<PutRoleRequest, Result<RoleModel>>
{
    public async Task<Result<RoleModel>> Handle(PutRoleRequest request, CancellationToken cancellationToken)
    {

        var roleResult = await dispatcher.DispatchAsync(new GetRolesQuery { AsNoTracking = true, Name = request.Model.Name });
        if (roleResult.Any(u => u.Id != request.RoleId))
            return Result.Failure("Role Already Exists");

        var existingRoleResult = await dispatcher.DispatchAsync(new GetRoleQuery { Id = request.RoleId });
        if (existingRoleResult == null)
            return Result.NotFound("Role not found");

        var updatedEntity = existingRoleResult.ToUpdatedEntity(request.Model);
        await dispatcher.DispatchAsync(new AddUpdateRoleCommand { Role = updatedEntity });

        var newModel = updatedEntity.ToModel();

        return Result.Success(newModel);
    }
}
