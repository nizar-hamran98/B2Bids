using MediatR;
using SharedKernel;
using Stores.Application.Queries;
using Stores.Domain.Models;
using Stores.Domain.Mapping;

namespace Stores.Application.RequestHandlers;
public class GetStoreAddressRequest : IRequest<IEnumerable<StoreAddressModel>?>
{
}

internal sealed class GetStoreAddressRequestHandler(Dispatcher dispatcher) : IRequestHandler<GetStoreAddressRequest, IEnumerable<StoreAddressModel>?>
{
    public async Task<IEnumerable<StoreAddressModel>?> Handle(GetStoreAddressRequest request, CancellationToken cancellationToken)
    {
        var result = await dispatcher.DispatchAsync(new StoreAddressQuery(), cancellationToken);
        return result?.ToModels();
    }
}
