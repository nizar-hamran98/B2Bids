using Identity.Domain.Entities;
using SharedKernel;

namespace Identity.Application.Commands;

public class DeleteRoleCommand : ICommand
{
    public required Role Role { get; set; }

    public class DeleteRoleCommandHandler(IRepositoryBase<Role> _repo, IUnitOfWork unitOfWork) : ICommandHandler<DeleteRoleCommand>
    {
        public async Task Handle(DeleteRoleCommand command, CancellationToken cancellationToken = default)
        {
            await _repo.DeleteAsync(command.Role);
            await unitOfWork.SaveChanges(cancellationToken);
        }
    }
}