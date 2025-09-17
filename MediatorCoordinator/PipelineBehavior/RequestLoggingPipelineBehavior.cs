﻿using MediatR;
using Microsoft.Extensions.Logging;
using SharedKernel;

namespace MediatorCoordinator;
internal sealed class RequestLoggingPipelineBehavior<TRequest, TResponse>(
  ILogger<RequestLoggingPipelineBehavior<TRequest, TResponse>> logger)
  : IPipelineBehavior<TRequest, TResponse>
  where TRequest : class
  where TResponse : ResultBase
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        string requestName = typeof(TRequest).Name;

        logger.LogInformation(
            "Processing request {RequestName}",
            requestName);

        TResponse result = await next();

        if (result.IsSuccess)
        {
            logger.LogInformation(
                "Completed request {RequestName}",
                requestName);
        }
        else
        {
            logger.LogError(
                   "Completed request {RequestName} with error",
                   requestName);

        }

        return result;
    }
}

