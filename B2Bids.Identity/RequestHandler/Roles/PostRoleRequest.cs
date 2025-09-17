using Identity.Application.Commands;
using Identity.Application.Queries;
using Identity.Domain.Mapping;
using Identity.Domain.Models;
using MediatorCoordinator.Contract;
using MediatR;
using SharedKernel;

public class PostRoleRequest : IRequestContext<Result<RoleModel>>
{
    public required RoleModel Model { get; set; }
}

public class PostRoleRequestHandler(Dispatcher _dispatcher) : IRequestHandler<PostRoleRequest, Result<RoleModel>>
{
    public async Task<Result<RoleModel>> Handle(PostRoleRequest request, CancellationToken cancellationToken)
    {

        var roleResult = await _dispatcher.DispatchAsync(new GetRoleQuery { Name = request.Model.Name });
        if (roleResult != null)
            return Result.Failure("Role Already Exists");



        var roleEntity = request.Model.ToEntity();

         await _dispatcher.DispatchAsync(new AddUpdateRoleCommand { Role = roleEntity });

        var newModel = roleEntity.ToModel();

        return Result.Success(newModel);
    }
}
