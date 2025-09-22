using MediatR;
using Products.Application.Queries;
using Products.Domain.Models;
using SharedKernel;
using Products.Domain.Mapping;

namespace Products.Application.RequestHandler;
public class GetCategoryByRequest : IRequest<CategoryModel>
{
    public long? Id { get; set; }
    public string? Name { get; set; }
}

internal sealed class GetCategoryByRequestHandler(Dispatcher dispatcher) : IRequestHandler<GetCategoryByRequest, CategoryModel>
{
    public async Task<CategoryModel> Handle(GetCategoryByRequest request, CancellationToken cancellationToken)
    {
        var result = await dispatcher.DispatchAsync(new CategoryQuery { AsNoTracking = true, Id = request.Id, Name = request.Name}, cancellationToken);
        return result.FirstOrDefault().ToModel();
    }
}
