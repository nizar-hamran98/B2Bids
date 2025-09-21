using MediatorCoordinator.Contract;
using MediatR;
using Products.Application.Commands;
using Products.Application.Queries;
using Products.Domain.Models;
using SharedKernel;
using Products.Domain.Mapping;

namespace Products.Application.RequestHandler;
public class AddUpdateCategoryRequest : IRequestContext<Result<CategoryModel>>
{
    public long? Id { get; set; }
    public required CategoryModel Model { get; set; }
}

internal sealed class AddUpdateCategoryRequestHandler(Dispatcher _dispatcher) : IRequestHandler<AddUpdateCategoryRequest, Result<CategoryModel>>
{
    public async Task<Result<CategoryModel>> Handle(AddUpdateCategoryRequest request, CancellationToken cancellationToken)
    {
        if (request.Id.HasValue)
        {
            var exists = await _dispatcher.DispatchAsync(new CategoryQuery { AsNoTracking = true, Id = request.Id });
            if (exists == null)
                return Result.Failure("Category does not exist!");

            var updated = exists.FirstOrDefault()?.ToUpdatedEntity(request.Model);
            await _dispatcher.DispatchAsync(new AddUpdateCategoryCommand { Category = updated });
            return Result.Success(updated.ToModel());
        }
        else
        {
            var entity = request.Model.ToEntity();
            await _dispatcher.DispatchAsync(new AddUpdateCategoryCommand { Category = entity });
            return Result.Success(entity.ToModel());
        }
    }
}
