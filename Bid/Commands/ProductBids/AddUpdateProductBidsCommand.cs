using Bid.Domain.Entities;
using SharedKernel;

namespace Bid.Application.Commands;
public class AddUpdateProductBidsCommand : ICommand
{
    public required ProductBids Entity { get; set; }
}

public class AddUpdateProductBidsCommandHandler(IRepositoryBase<ProductBids> repo, IUnitOfWork unitOfWork) : ICommandHandler<AddUpdateProductBidsCommand>
{
    public async Task Handle(AddUpdateProductBidsCommand command, CancellationToken cancellationToken = default)
    {
        if (command.Entity.Id > 0)
            await repo.UpdateAsync(command.Entity);
        else
            await repo.AddAsync(command.Entity);

        await unitOfWork.SaveChanges(cancellationToken);
    }
}