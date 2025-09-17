using Identity.Domain.Entities;
using SharedKernel;

namespace Identity.Application.Commands;

public class AddUpdateUserCommand : ICommand
{
    public User User { get; set; }
    public List<User> Users { get; set; }
}
public class AddUpdateUserCommandHandler(IRepositoryBase<User> repository, IUnitOfWork unitOfWork) : ICommandHandler<AddUpdateUserCommand>
{
    public async Task Handle(AddUpdateUserCommand command, CancellationToken cancellationToken)
    {
        if (command.User != null)
            await repository.AddOrUpdateAsync(command.User, cancellationToken);
        else if (command.Users != null && command.Users.Count > 0)
            await repository.UpdateRangeAsync(command.Users);

        await unitOfWork.SaveChanges();
    }

}
