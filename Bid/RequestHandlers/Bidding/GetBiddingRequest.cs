using Bid.Application.Queries;
using Bid.Domain.Mapping;
using Bid.Domain.Models;
using MediatR;
using SharedKernel;

namespace Bid.Application.RequestHandlers;
public class GetBiddingRequest : IRequest<IEnumerable<BiddingModel>>
{
}

internal sealed class GetBiddingRequestHandler(Dispatcher dispatcher) : IRequestHandler<GetBiddingRequest, IEnumerable<BiddingModel>>
{
    public async Task<IEnumerable<BiddingModel>> Handle(GetBiddingRequest request, CancellationToken cancellationToken)
    {
        var result = await dispatcher.DispatchAsync(new BiddingQuery(), cancellationToken);
        return result.ToModels();
    }
}