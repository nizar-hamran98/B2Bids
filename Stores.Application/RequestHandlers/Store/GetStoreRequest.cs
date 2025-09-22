using MediatR;
using SharedKernel;
using Stores.Application.Queries;
using Stores.Domain.Models;
using Stores.Domain.Mapping;

namespace Stores.Application.RequestHandlers;
public class GetStoreRequest : IRequest<IEnumerable<StoreModel>?>
{
}

internal sealed class GetStoreRequestHandler(Dispatcher dispatcher) : IRequestHandler<GetStoreRequest, IEnumerable<StoreModel>?>
{
    public async Task<IEnumerable<StoreModel>?> Handle(GetStoreRequest request, CancellationToken cancellationToken)
    {
        var result = await dispatcher.DispatchAsync(new StoreQuery(), cancellationToken);
        return result?.ToModels();
    }
}
