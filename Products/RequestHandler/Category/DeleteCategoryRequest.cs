using MediatorCoordinator.Contract;
using MediatR;
using Products.Application.Commands;
using Products.Application.Queries;
using SharedKernel;

namespace Products.Application.RequestHandler;
public class DeleteCategoryRequest : IRequestContext<Result<bool>>
{
    public long Id { get; set; }
}

internal sealed class DeleteCategoryRequestHandler(Dispatcher dispatcher) : IRequestHandler<DeleteCategoryRequest, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeleteCategoryRequest request, CancellationToken cancellationToken)
    {
        var exists = await dispatcher.DispatchAsync(new CategoryQuery { AsNoTracking = true, Id = request.Id });
        if (exists.FirstOrDefault() == null)
            return Result.Failure("Category does not exist!");

        await dispatcher.DispatchAsync(new DeleteCategoryCommand { Category = exists.FirstOrDefault() }, cancellationToken);
        return Result.Success("Successfully Deleted");
    }
}
