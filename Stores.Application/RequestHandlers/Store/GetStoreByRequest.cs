using MediatR;
using SharedKernel;
using Stores.Application.Queries;
using Stores.Domain.Models;
using Stores.Domain.Mapping;

namespace Stores.Application.RequestHandlers;
public class GetStoreByRequest : IRequest<StoreModel?>
{
    public long? Id { get; set; }
    public string? Name { get; set; }
}

internal sealed class GetCategoryByRequestHandler(Dispatcher dispatcher) : IRequestHandler<GetStoreByRequest, StoreModel?>
{
    public async Task<StoreModel?> Handle(GetStoreByRequest request, CancellationToken cancellationToken)
    {
        var result = await dispatcher.DispatchAsync(new StoreQuery { AsNoTracking = false, Id = request.Id, Name = request.Name , IncludeShortAddress = true }, cancellationToken);
        return result.FirstOrDefault()?.ToModel();
    }
}