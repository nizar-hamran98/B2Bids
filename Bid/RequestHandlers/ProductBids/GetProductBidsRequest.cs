using Bid.Application.Queries;
using Bid.Domain.Mapping;
using Bid.Domain.Models;
using MediatR;
using SharedKernel;

namespace Bid.Application.RequestHandlers;
public class GetProductBidsRequest : IRequest<IEnumerable<ProductBidsModel>>
{
}

internal sealed class GetProductBidsRequestHandler(Dispatcher dispatcher) : IRequestHandler<GetProductBidsRequest, IEnumerable<ProductBidsModel>>
{
    public async Task<IEnumerable<ProductBidsModel>> Handle(GetProductBidsRequest request, CancellationToken cancellationToken)
    {
        var result = await dispatcher.DispatchAsync(new ProductBidsQuery(), cancellationToken);
        return result.ToModels();
    }
}