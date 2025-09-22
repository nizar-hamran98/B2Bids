using MediatorCoordinator.Contract;
using MediatR;
using Products.Application.Commands;
using Products.Application.Queries;
using SharedKernel;

namespace Products.Application.RequestHandler;
public class DeleteProductRequest : IRequestContext<Result<bool>>
{
    public long Id { get; set; }
}

internal sealed class DeleteProductRequestHandler(Dispatcher dispatcher) : IRequestHandler<DeleteProductRequest, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeleteProductRequest request, CancellationToken cancellationToken)
    {
        var exists = await dispatcher.DispatchAsync(new ProductQuery { AsNoTracking = true, Id = request.Id });
        if (exists.FirstOrDefault() == null)
            return Result.Failure("Product does not exist!");

        await dispatcher.DispatchAsync(new DeleteProductCommand { Product = exists.FirstOrDefault() }, cancellationToken);
        return Result.Success("Successfully Deleted");
    }
}
