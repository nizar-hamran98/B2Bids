using MediatR;
using Products.Application.Queries;
using Products.Domain.Mapping;
using Products.Domain.Models;
using SharedKernel;

namespace Products.Application.RequestHandler;
public class GetCategoryRequest : IRequest<IEnumerable<CategoryModel>>
{
}

internal sealed class GetCategoryRequestHandler(Dispatcher dispatcher) : IRequestHandler<GetCategoryRequest, IEnumerable<CategoryModel>>
{
    public async Task<IEnumerable<CategoryModel>> Handle(GetCategoryRequest request, CancellationToken cancellationToken)
    {
        var result = await dispatcher.DispatchAsync(new CategoryQuery(), cancellationToken);
        return result.ToModels();
    }
}
