using Bid.Domain.Entities;
using SharedKernel;

namespace Bid.Application.Commands;
public class DeleteBiddingCommand : ICommand
{
    public required Bidding Entity { get; set; }
}

public class DeleteBiddingCommandHandler(IRepositoryBase<Bidding> repo, IUnitOfWork unitOfWork) : ICommandHandler<DeleteBiddingCommand>
{
    public async Task Handle(DeleteBiddingCommand command, CancellationToken cancellationToken = default)
    {
        await repo.DeleteAsync(command.Entity, cancellationToken);
        await unitOfWork.SaveChanges(cancellationToken);
    }
}
