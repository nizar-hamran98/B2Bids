using MediatorCoordinator.Contract;
using MediatR;
using SharedKernel;
using Stores.Application.Queries;
using Stores.Domain.Models;
using Stores.Domain.Mapping;
using Stores.Application.Commands;

namespace Stores.Application.RequestHandlers;
public class AddUpdateStoreRequest : IRequestContext<Result<StoreModel>>
{
    public long? Id { get; set; }
    public required StoreModel Model { get; set; }
}

internal sealed class AddUpdateStoreRequestHandler(Dispatcher _dispatcher) : IRequestHandler<AddUpdateStoreRequest, Result<StoreModel>>
{
    public async Task<Result<StoreModel>> Handle(AddUpdateStoreRequest request, CancellationToken cancellationToken)
    {
        if (request.Id.HasValue)
        {
            var exists = await _dispatcher.DispatchAsync(new StoreQuery { AsNoTracking = true, Id = request.Id });
            if (exists == null)
                return Result.Failure("Store does not exist!");

            var updated = exists.FirstOrDefault()?.ToUpdatedEntity(request.Model);
            await _dispatcher.DispatchAsync(new AddUpdateStoreCommand { Entity = updated });
            return Result.Success(updated.ToModel());
        }
        else
        {
            var entity = request.Model.ToEntity();
            await _dispatcher.DispatchAsync(new AddUpdateStoreCommand { Entity = entity });
            return Result.Success(entity.ToModel());
        }
    }
}