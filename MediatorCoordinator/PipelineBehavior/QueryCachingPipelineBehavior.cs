using MediatR;
using Microsoft.Extensions.Logging;
using SharedKernel;

namespace MediatorCoordinator;

internal sealed class QueryCachingPipelineBehavior<TRequest, TResponse>
        (IRedisHash redisHash, ILogger<QueryCachingPipelineBehavior<TRequest, TResponse>> logger)
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : ICachedQuery
        where TResponse : ResultBase
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        string requestName = typeof(TRequest).Name;
        if (request.CacheKey?.Length > 0)
        {
            var key = redisHash.CreateCacheKey(request.CacheKey);

            var cachedResult = await redisHash.GetValueAsync<TResponse>(key);

            if (cachedResult is not null)
            {
                logger.LogInformation("Cache hit for {RequestName}", requestName);

                return cachedResult;
            }
        }

        logger.LogInformation("Cache miss for {RequestName}", requestName);

        TResponse response = await next();

        if (response.IsSuccess && request.CacheKey?.Length > 0)
        {
            redisHash.Insert(response,request.CacheKey);
        }

        return response;
    }
}
