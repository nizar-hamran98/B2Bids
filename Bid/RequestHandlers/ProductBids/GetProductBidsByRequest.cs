using Bid.Application.Queries;
using Bid.Domain.Mapping;
using Bid.Domain.Models;
using MediatR;
using SharedKernel;

namespace Bid.Application.RequestHandlers;
public class GetProductBidsByRequest : IRequest<ProductBidsModel>
{
    public long? Id { get; set; }
    public long? ProductId { get; set; }
    public long? WinnerId { get; set; }
    public long? ListOfBidders { get; set; }
}

internal sealed class GetProductBidsByRequestHandler(Dispatcher dispatcher) : IRequestHandler<GetProductBidsByRequest, ProductBidsModel>
{
    public async Task<ProductBidsModel> Handle(GetProductBidsByRequest request, CancellationToken cancellationToken)
    {
        var result = await dispatcher.DispatchAsync(new ProductBidsQuery { AsNoTracking = false, Id = request.Id, WinnerId = request.WinnerId, ListOfBidders = request.ListOfBidders }, cancellationToken);
        return result.FirstOrDefault().ToModel();
    }
}
