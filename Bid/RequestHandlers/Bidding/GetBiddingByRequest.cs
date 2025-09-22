using Bid.Application.Queries;
using Bid.Domain.Mapping;
using Bid.Domain.Models;
using MediatR;
using SharedKernel;

namespace Bid.Application.RequestHandlers;
public class GetBiddingByRequest : IRequest<BiddingModel>
{
    public long? Id { get; set; }
    public long? ProductId { get; set; }
    public long? BidderId { get; set; }
}

internal sealed class GetBiddingByRequestHandler(Dispatcher dispatcher) : IRequestHandler<GetBiddingByRequest, BiddingModel>
{
    public async Task<BiddingModel> Handle(GetBiddingByRequest request, CancellationToken cancellationToken)
    {
        var result = await dispatcher.DispatchAsync(new BiddingQuery { AsNoTracking = false, Id = request.Id, ProductId = request.ProductId, BidderId = request.BidderId }, cancellationToken);
        return result.FirstOrDefault().ToModel();
    }
}
