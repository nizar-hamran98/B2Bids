using Bid.Application.Commands;
using Bid.Application.Queries;
using Bid.Domain.Mapping;
using Bid.Domain.Models;
using MediatorCoordinator.Contract;
using MediatR;
using SharedKernel;

namespace Bid.Application.RequestHandlers;
public class AddUpdateProductBidsRequest : IRequestContext<Result<ProductBidsModel>>
{
    public long? Id { get; set; }
    public required ProductBidsModel Model { get; set; }
}

internal sealed class AddUpdateProductBidsRequestHandler(Dispatcher _dispatcher) : IRequestHandler<AddUpdateProductBidsRequest, Result<ProductBidsModel>>
{
    public async Task<Result<ProductBidsModel>> Handle(AddUpdateProductBidsRequest request, CancellationToken cancellationToken)
    {
        if (request.Id.HasValue)
        {
            var exists = await _dispatcher.DispatchAsync(new ProductBidsQuery { AsNoTracking = true, Id = request.Id });
            if (exists == null)
                return Result.Failure("ProductBids does not exist!");

            var updated = exists.FirstOrDefault()?.ToUpdatedEntity(request.Model);
            await _dispatcher.DispatchAsync(new AddUpdateProductBidsCommand { Entity = updated });
            return Result.Success(updated.ToModel());
        }
        else
        {
            var entity = request.Model.ToEntity();
            await _dispatcher.DispatchAsync(new AddUpdateProductBidsCommand { Entity = entity });
            return Result.Success(entity.ToModel());
        }
    }
}

