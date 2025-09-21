using SharedKernel;
using Stores.Domain.Entities;

namespace Stores.Application.Commands;
public class DeleteStoreCommand : ICommand
{
    public required Store Entity { get; set; }
}

public class DeleteStoreCommandHandler(IRepositoryBase<Store> repo, IUnitOfWork unitOfWork) : ICommandHandler<DeleteStoreCommand>
{
    public async Task Handle(DeleteStoreCommand command, CancellationToken cancellationToken = default)
    {
        await repo.DeleteAsync(command.Entity, cancellationToken);
        await unitOfWork.SaveChanges(cancellationToken);
    }
}