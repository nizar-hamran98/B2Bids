using FluentValidation;
using MediatR;
using SharedKernel;

namespace MediatorCoordinator;
internal sealed class ValidationPipelineBehavior<TRequest, TResponse>(
      IEnumerable<IValidator<TRequest>> validators)
  : IPipelineBehavior<TRequest, TResponse>

  where TRequest : class

  where TResponse : class

{
   public async Task<TResponse> Handle(
        TRequest request,

        RequestHandlerDelegate<TResponse> next,

        CancellationToken cancellationToken)

    {

        var validationFailure = await ValidateAsync(request);
        if (validationFailure.IsFailed)

        {
            var errorResponse = ValidationPipelineBehavior<TRequest, TResponse>.CreateErrorResponse(validationFailure);
            return errorResponse;
        }
        return await next();

    }

    private async Task<Result> ValidateAsync(TRequest request)

    {

        var validator = validators.FirstOrDefault();
        if (validator is null)

        {

            return Result.Success();

        }
        var context = new ValidationContext<TRequest>(request);
        var result = await validator.ValidateAsync(context);
        if (result.IsFailed())

            return result.Invalid();
        return Result.Success();
    }
    private static TResponse CreateErrorResponse(Result validationFailure)
    {
        // Assuming TResponse is Result<T> where T is the actual type (e.g., MyDataType)
        var responseType = typeof(TResponse);
        if (responseType.IsGenericType && responseType.GetGenericTypeDefinition() == typeof(Result<>))
        {
            var genericArgumentType = responseType.GetGenericArguments()[0]; // Get T from Result<T>
            // Dynamically create an instance of Result<T> with null Data and the validation failure
            var resultType = typeof(Result<>).MakeGenericType(genericArgumentType);
            // Use reflection to create a new Result<T> instance and initialize properties
            var errorResult = Activator.CreateInstance(resultType);
            // Set the properties using reflection (Data = null, Errors from validationFailure, IsSuccess = false)
            resultType.GetProperty(nameof(Result<object>.Data))!.SetValue(errorResult, null);
            resultType.GetProperty(nameof(ResultBase.Errors))!.SetValue(errorResult, validationFailure.Errors);
            resultType.GetProperty(nameof(ResultBase.IsSuccess))!.SetValue(errorResult, false);
            resultType.GetProperty(nameof(ResultBase.Status))!.SetValue(errorResult, ResultStatus.Failure);
            resultType.GetProperty(nameof(ResultBase.Message))!.SetValue(errorResult, validationFailure.Message);
            return (TResponse)errorResult;
        }
        throw new InvalidOperationException("TResponse must be of type Result<T>");
    }
}