using MediatorCoordinator.Contract;
using MediatR;
using SharedKernel;
using Stores.Application.Commands;
using Stores.Application.Queries;
using Stores.Domain.Models;
using Stores.Domain.Mapping;

namespace Stores.Application.RequestHandlers;
public class AddUpdateStoreAddressRequest : IRequestContext<Result<StoreAddressModel>>
{
    public long? Id { get; set; }
    public required StoreAddressModel Model { get; set; }
}

internal sealed class AddUpdateStoreAddressRequestHandler(Dispatcher _dispatcher) : IRequestHandler<AddUpdateStoreAddressRequest, Result<StoreAddressModel>>
{
    public async Task<Result<StoreAddressModel>> Handle(AddUpdateStoreAddressRequest request, CancellationToken cancellationToken)
    {
        if (request.Id.HasValue)
        {
            var exists = await _dispatcher.DispatchAsync(new StoreAddressQuery { AsNoTracking = true, Id = request.Id });
            if (exists == null)
                return Result.Failure("Store Address does not exist!");

            var updated = exists.FirstOrDefault()?.ToUpdatedEntity(request.Model);
            await _dispatcher.DispatchAsync(new AddUpdateStoreAddressCommand { Entity = updated });
            return Result.Success(updated.ToModel());
        }
        else
        {
            var entity = request.Model.ToEntity();
            await _dispatcher.DispatchAsync(new AddUpdateStoreAddressCommand { Entity = entity });
            return Result.Success(entity.ToModel());
        }
    }
}
