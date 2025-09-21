using SharedKernel;
using Stores.Domain.Entities;

namespace Stores.Application.Commands;
public class AddUpdateStoreAddressCommand : ICommand
{
    public required StoreAddress Entity { get; set; }
}

public class AddUpdateStoreAddressHandler(IRepositoryBase<StoreAddress> repo, IUnitOfWork unitOfWork) : ICommandHandler<AddUpdateStoreAddressCommand>
{
    public async Task Handle(AddUpdateStoreAddressCommand command, CancellationToken cancellationToken = default)
    {
        if (command.Entity.Id > 0)
            await repo.UpdateAsync(command.Entity);
        else
            await repo.AddAsync(command.Entity);

        await unitOfWork.SaveChanges(cancellationToken);
    }
}