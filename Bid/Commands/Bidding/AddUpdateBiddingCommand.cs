using Bid.Domain.Entities;
using SharedKernel;

namespace Bid.Application.Commands;
public class AddUpdateBiddingCommand : ICommand
{
    public required Bidding Entity { get; set; }
}

public class AddUpdateBiddingCommandHandler(IRepositoryBase<Bidding> repo, IUnitOfWork unitOfWork) : ICommandHandler<AddUpdateBiddingCommand>
{
    public async Task Handle(AddUpdateBiddingCommand command, CancellationToken cancellationToken = default)
    {
        if (command.Entity.Id > 0)
            await repo.UpdateAsync(command.Entity);
        else
            await repo.AddAsync(command.Entity);

        await unitOfWork.SaveChanges(cancellationToken);
    }
}
