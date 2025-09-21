using MediatR;
using SharedKernel;
using Stores.Application.Queries;
using Stores.Domain.Models;
using Stores.Domain.Mapping;

namespace Stores.Application.RequestHandlers;
public class GetStoreAddressByRequest : IRequest<StoreAddressModel?>
{
    public long? Id { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
}

internal sealed class GetStoreAddressByRequestHandler(Dispatcher dispatcher) : IRequestHandler<GetStoreAddressByRequest, StoreAddressModel?>
{
    public async Task<StoreAddressModel?> Handle(GetStoreAddressByRequest request, CancellationToken cancellationToken)
    {
        var result = await dispatcher.DispatchAsync(new StoreAddressQuery { AsNoTracking = false, Id = request.Id, City = request.City, Country = request.Country }, cancellationToken);

        return result.FirstOrDefault()?.ToModel();
    }
}