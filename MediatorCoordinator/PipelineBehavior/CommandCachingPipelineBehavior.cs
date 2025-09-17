using MediatorCoordinator.Contract;
using MediatR;
using SharedKernel;

namespace MediatorCoordinator.PipelineBehavior;
internal sealed class CommandCachingPipelineBehavior<TRequest, TResponse>
        () : IPipelineBehavior<TRequest, TResponse>
        where TRequest : ICachedCommand
        where TResponse : ResultBase
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        TResponse response = await next();
        if (response.IsSuccess)
        {
            var resultType = response.GetType();
            if (resultType.IsGenericType && resultType.GetGenericTypeDefinition() == typeof(Result<>))
            {
                var entityValue = resultType.GetProperty("Data")?.GetValue(response);
                var entityType = entityValue?.GetType();

                if (entityValue != null)
                {
                    redisHash.Delete(entityValue, entityType!.Name);
                }
            }
        }
        return response;
    }
}
