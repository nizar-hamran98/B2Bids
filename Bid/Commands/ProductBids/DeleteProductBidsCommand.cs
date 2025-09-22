using Bid.Domain.Entities;
using SharedKernel;

namespace Bid.Application.Commands;
public class DeleteProductBidsCommand : ICommand
{
    public required ProductBids Entity { get; set; }
}

public class DeleteProductBidsCommandHandler(IRepositoryBase<ProductBids> repo, IUnitOfWork unitOfWork) : ICommandHandler<DeleteProductBidsCommand>
{
    public async Task Handle(DeleteProductBidsCommand command, CancellationToken cancellationToken = default)
    {
        await repo.DeleteAsync(command.Entity, cancellationToken);
        await unitOfWork.SaveChanges(cancellationToken);
    }
}
