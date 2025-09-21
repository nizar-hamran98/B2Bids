using MediatorCoordinator.Contract;
using MediatR;
using SharedKernel;
using Stores.Application.Commands;
using Stores.Application.Queries;

namespace Stores.Application.RequestHandlers;
public class DeleteStoreRequest : IRequestContext<Result<bool>>
{
    public long Id { get; set; }
}

internal sealed class DeleteStoreRequestHandler(Dispatcher dispatcher) : IRequestHandler<DeleteStoreRequest, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeleteStoreRequest request, CancellationToken cancellationToken)
    {
        var exists = await dispatcher.DispatchAsync(new StoreQuery { AsNoTracking = true, Id = request.Id });
        if (exists.FirstOrDefault() == null)
            return Result.Failure("Store does not exist!");

        await dispatcher.DispatchAsync(new DeleteStoreCommand { Entity = exists.FirstOrDefault() }, cancellationToken);
        return Result.Success("Successfully Deleted");
    }
}
