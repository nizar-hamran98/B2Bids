using Identity.Domain.Entities;
using SharedKernel;

namespace Identity.Application.Commands;

public class DeleteRolePermissionCommand : ICommand
{
    public required RolePermissions RolePermission { get; set; }
}

public class DeleteRolePermissionCommandHandler(IRepositoryBase<RolePermissions> repository, IUnitOfWork unitOfWork) : ICommandHandler<DeleteRolePermissionCommand>
{
    public async Task Handle(DeleteRolePermissionCommand command, CancellationToken cancellationToken = default)
    {
         await repository.DeleteAsync(command.RolePermission, cancellationToken);
         await unitOfWork.SaveChanges(cancellationToken);
    }
}