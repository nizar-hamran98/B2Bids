using SharedKernel;
using Stores.Domain.Entities;

namespace Stores.Application.Commands;
public class AddUpdateStoreCommand : ICommand
{
    public required Store Entity { get; set; }
}

public class AddUpdateStoreHandler(IRepositoryBase<Store> repo, IUnitOfWork unitOfWork) : ICommandHandler<AddUpdateStoreCommand>
{
    public async Task Handle(AddUpdateStoreCommand command, CancellationToken cancellationToken = default)
    {
        if (command.Entity.Id > 0)
            await repo.UpdateAsync(command.Entity);
        else
            await repo.AddAsync(command.Entity);

        await unitOfWork.SaveChanges(cancellationToken);
    }
}