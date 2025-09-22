using Bid.Application.Commands;
using Bid.Application.Queries;
using MediatorCoordinator.Contract;
using MediatR;
using SharedKernel;

namespace Bid.Application.RequestHandlers;
public class DeleteProductBidsRequest : IRequestContext<Result<bool>>
{
    public long Id { get; set; }
}

internal sealed class DeleteProductBidsRequestHandler(Dispatcher dispatcher) : IRequestHandler<DeleteProductBidsRequest, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeleteProductBidsRequest request, CancellationToken cancellationToken)
    {
        var exists = await dispatcher.DispatchAsync(new ProductBidsQuery { AsNoTracking = true, Id = request.Id });
        if (exists.FirstOrDefault() == null)
            return Result.Failure("ProductBids does not exist!");

        await dispatcher.DispatchAsync(new DeleteProductBidsCommand { Entity = exists.FirstOrDefault() }, cancellationToken);
        return Result.Success("Successfully Deleted");
    }
}

