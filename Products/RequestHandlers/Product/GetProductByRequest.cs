using MediatR;
using Products.Application.Queries;
using Products.Domain.Models;
using SharedKernel;
using Products.Domain.Mapping;

namespace Products.Application.RequestHandler;
public class GetProductByRequest : IRequest<ProductModel>
{
    public long? Id { get; set; }
    public string? Name { get; set; }
    public long? StoreId { get; set; }
    public long? CategoryId { get; set; }
}

internal sealed class GetProductByRequestHandler(Dispatcher dispatcher) : IRequestHandler<GetProductByRequest, ProductModel>
{
    public async Task<ProductModel> Handle(GetProductByRequest request, CancellationToken cancellationToken)
    {
        var result = await dispatcher.DispatchAsync(new ProductQuery { AsNoTracking = true , CategoryId = request.CategoryId , Id = request.Id, Name = request.Name , StoreId = request.StoreId }, cancellationToken);
        return result.FirstOrDefault().ToModel();
    }
}
