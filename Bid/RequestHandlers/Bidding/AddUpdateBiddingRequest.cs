using Bid.Application.Commands;
using Bid.Application.Queries;
using Bid.Domain.Mapping;
using Bid.Domain.Models;
using MediatorCoordinator.Contract;
using MediatR;
using SharedKernel;

namespace Bid.Application.RequestHandlers;
public class AddUpdateBiddingRequest : IRequestContext<Result<BiddingModel>>
{
    public long? Id { get; set; }
    public required BiddingModel Model { get; set; }
}

internal sealed class AddUpdateBiddingRequestHandler(Dispatcher _dispatcher) : IRequestHandler<AddUpdateBiddingRequest, Result<BiddingModel>>
{
    public async Task<Result<BiddingModel>> Handle(AddUpdateBiddingRequest request, CancellationToken cancellationToken)
    {
        if (request.Id.HasValue)
        {
            var exists = await _dispatcher.DispatchAsync(new BiddingQuery { AsNoTracking = true, Id = request.Id });
            if (exists == null)
                return Result.Failure("Bidding does not exist!");

            var updated = exists.FirstOrDefault()?.ToUpdatedEntity(request.Model);
            await _dispatcher.DispatchAsync(new AddUpdateBiddingCommand { Entity = updated });
            return Result.Success(updated.ToModel());
        }
        else
        {
            var entity = request.Model.ToEntity();
            await _dispatcher.DispatchAsync(new AddUpdateBiddingCommand { Entity = entity });
            return Result.Success(entity.ToModel());
        }
    }
}

