using Identity.Domain.Entities;
using SharedKernel;

namespace Identity.Application.Commands;
public class AddUpdateUserLoginCommand : ICommand
{
    public required UserLogin UserLogin { get; set; }
}
public class AddUpdateUserLoginCommandHandler(IRepositoryBase<UserLogin> repository, IUnitOfWork unitOfWork) : ICommandHandler<AddUpdateUserLoginCommand>
{
    public async Task Handle(AddUpdateUserLoginCommand command, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(command.UserLogin.UserAgent))
            command.UserLogin.UserAgent = "-";

        await repository.AddOrUpdateAsync(command.UserLogin);

        await unitOfWork.SaveChanges();
    }

}
