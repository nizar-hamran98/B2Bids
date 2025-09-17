using Identity.Domain.Entities;
using SharedKernel;

namespace Identity.Application.Commands;
public class AddUpdateRoleCommand : ICommand
{
    public required Role Role { get; set; }
}
public class AddUpdateRoleCommandHandler(IRepositoryBase<Role> role, IUnitOfWork unitOfWork) : ICommandHandler<AddUpdateRoleCommand>
{
    public async Task Handle(AddUpdateRoleCommand command, CancellationToken cancellationToken = default)
    {
        await role.AddOrUpdateAsync(command.Role);
        await unitOfWork.SaveChanges(cancellationToken);
    }
}
