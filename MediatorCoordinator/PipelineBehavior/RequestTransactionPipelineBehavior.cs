using MediatorCoordinator.Contract;
using MediatR;
using SharedKernel;

namespace MediatorCoordinator.PipelineBehavior;
internal sealed class RequestTransactionPipelineBehavior<TRequest, TResponse>(IUnitOfWork unitOfWork)
    : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequestContext
        where TResponse : ResultBase
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        unitOfWork.BeginTransaction();
        TResponse response = await next();

        if (response.IsSuccess)
            await unitOfWork.Commit(cancellationToken);

        else
            await unitOfWork.Rollback(cancellationToken);

        return response;
    }
}