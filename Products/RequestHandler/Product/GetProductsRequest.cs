using MediatR;
using Products.Application.Queries;
using Products.Domain.Mapping;
using Products.Domain.Models;
using SharedKernel;

namespace Products.Application.RequestHandler;
public class GetProductsRequest : IRequest<IEnumerable<ProductModel>>
{
}

internal sealed class GetProductsRequestHandler(Dispatcher dispatcher) : IRequestHandler<GetProductsRequest, IEnumerable<ProductModel>>
{
    public async Task<IEnumerable<ProductModel>> Handle(GetProductsRequest request, CancellationToken cancellationToken)
    {
        var result = await dispatcher.DispatchAsync(new ProductQuery(), cancellationToken);
        return result.ToModels();
    }
}
