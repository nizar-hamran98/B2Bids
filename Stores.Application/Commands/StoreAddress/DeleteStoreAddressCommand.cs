using SharedKernel;
using Stores.Domain.Entities;

namespace Stores.Application.Commands;
public class DeleteStoreAddressCommand : ICommand
{
    public required StoreAddress Entity { get; set; }
}

public class DeleteStoreAddressCommandHandler(IRepositoryBase<StoreAddress> repo, IUnitOfWork unitOfWork) : ICommandHandler<DeleteStoreAddressCommand>
{
    public async Task Handle(DeleteStoreAddressCommand command, CancellationToken cancellationToken = default)
    {
        await repo.DeleteAsync(command.Entity, cancellationToken);
        await unitOfWork.SaveChanges(cancellationToken);
    }
}

