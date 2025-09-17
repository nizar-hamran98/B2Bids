using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Identity.Application.Commands;

public class DeleteUserPermissionCommand : ICommand
{
    public required UserPermissions UserPermissions { get; set; }
}

public class DeleteUserPermissionCommandHandler(IRepositoryBase<UserPermissions> permissionRepository, IUnitOfWork unitOfWork) : ICommandHandler<DeleteUserPermissionCommand>
{
    public async Task Handle(DeleteUserPermissionCommand command, CancellationToken cancellationToken = default)
    {
        var userPermission = await permissionRepository.GetAll().Where(u => u == command.UserPermissions).FirstOrDefaultAsync();

        await permissionRepository.DeleteAsync(command.UserPermissions, cancellationToken);
        await unitOfWork.SaveChanges(cancellationToken);
    }
}