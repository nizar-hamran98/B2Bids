using Bid.Application.Commands;
using Bid.Application.Queries;
using MediatorCoordinator.Contract;
using MediatR;
using SharedKernel;

namespace Bid.Application.RequestHandlers;
public class DeleteBiddingRequest : IRequestContext<Result<bool>>
{
    public long Id { get; set; }
}

internal sealed class DeleteBiddingRequestHandler(Dispatcher dispatcher) : IRequestHandler<DeleteBiddingRequest, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeleteBiddingRequest request, CancellationToken cancellationToken)
    {
        var exists = await dispatcher.DispatchAsync(new BiddingQuery { AsNoTracking = true, Id = request.Id });
        if (exists.FirstOrDefault() == null)
            return Result.Failure("Bidding does not exist!");

        await dispatcher.DispatchAsync(new DeleteBiddingCommand { Entity = exists.FirstOrDefault() }, cancellationToken);
        return Result.Success("Successfully Deleted");
    }
}

