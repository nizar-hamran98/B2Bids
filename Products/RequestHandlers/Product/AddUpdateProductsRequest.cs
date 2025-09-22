using MediatorCoordinator.Contract;
using MediatR;
using Products.Application.Commands;
using Products.Application.Queries;
using Products.Domain.Mapping;
using Products.Domain.Models;
using SharedKernel;

namespace Products.Application.RequestHandler;
public class AddUpdateProductsRequest : IRequestContext<Result<ProductModel>>
{
    public long? Id { get; set; }
    public required ProductModel Model { get; set; }
}

internal sealed class AddUpdateProductsRequestHandler(Dispatcher _dispatcher) : IRequestHandler<AddUpdateProductsRequest, Result<ProductModel>>
{
    public async Task<Result<ProductModel>> Handle(AddUpdateProductsRequest request, CancellationToken cancellationToken)
    {
        if (request.Id.HasValue)
        {
            var exists = await _dispatcher.DispatchAsync(new ProductQuery { AsNoTracking = true, Id = request.Id });
            if(exists == null)
                return Result.Failure("Product does not exist!");

            var updated = exists.FirstOrDefault()?.ToUpdatedEntity(request.Model);
             await _dispatcher.DispatchAsync(new AddUpdateProductCommand { Product = updated });
            return Result.Success(updated.ToModel());
        }
        else
        {
            var entity = request.Model.ToEntity();
            await _dispatcher.DispatchAsync(new AddUpdateProductCommand { Product = entity });
            return Result.Success(entity.ToModel());
        }
    }
}
