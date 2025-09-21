using MediatorCoordinator.Contract;
using MediatR;
using SharedKernel;
using Stores.Application.Commands;
using Stores.Application.Queries;

namespace Stores.Application.RequestHandlers;
public class DeleteStoreAddressRequest : IRequestContext<Result<bool>>
{
    public long Id { get; set; }
}

internal sealed class DeleteStoreAddressRequestHandler(Dispatcher dispatcher) : IRequestHandler<DeleteStoreAddressRequest, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeleteStoreAddressRequest request, CancellationToken cancellationToken)
    {
        var exists = await dispatcher.DispatchAsync(new StoreAddressQuery { AsNoTracking = true, Id = request.Id });
        if (exists.FirstOrDefault() == null)
            return Result.Failure("Store Address does not exist!");

        await dispatcher.DispatchAsync(new DeleteStoreAddressCommand { Entity = exists.FirstOrDefault() }, cancellationToken);
        return Result.Success("Successfully Deleted");
    }
}