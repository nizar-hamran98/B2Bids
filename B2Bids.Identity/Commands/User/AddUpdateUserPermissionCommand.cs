using Identity.Domain.Entities;
using SharedKernel;

namespace Identity.Application.Commands;

public class AddUpdateUserPermissionCommand : ICommand
{
    public required UserPermissions UserPermissions { get; set; }
}

public class AddUpdateUserPermissionCommandHandler(IRepositoryBase<UserPermissions> repository, IUnitOfWork unitOfWork) : ICommandHandler<AddUpdateUserPermissionCommand>
{
    public async Task Handle(AddUpdateUserPermissionCommand command, CancellationToken cancellationToken = default)
    {
        if (command.UserPermissions.Id > 0)
            await repository.UpdateAsync(command.UserPermissions);
        else
            await repository.AddAsync(command.UserPermissions);
        await unitOfWork.SaveChanges(cancellationToken);
    }
}