using Identity.Domain.Entities;
using SharedKernel;

namespace Identity.Application.Commands;

public class AddUpdateRolePermissionCommand : ICommand
{
    public required RolePermissions RolePermission { get; set; }
}

public class AddUpdateRolePermissionCommandHandler(IRepositoryBase<RolePermissions> rolePermissionsRepo, IUnitOfWork unitOfWork) : ICommandHandler<AddUpdateRolePermissionCommand>
{
    public async Task Handle(AddUpdateRolePermissionCommand command, CancellationToken cancellationToken = default)
    {
        if (command.RolePermission.Id > 0)
            await rolePermissionsRepo.UpdateAsync(command.RolePermission);
        else
             await rolePermissionsRepo.AddAsync(command.RolePermission);

         await unitOfWork.SaveChanges(cancellationToken);
       
    }
}