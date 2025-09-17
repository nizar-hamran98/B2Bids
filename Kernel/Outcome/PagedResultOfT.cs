using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace SharedKernel;


/// <summary>
/// Represents the paged result of an operation.
/// </summary>
/// <typeparam name="T">The type of objects to enumerate.</typeparam>
public sealed class PagedResult<T> : ResultBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PagedResult{T}"/> class.
    /// </summary>
    public PagedResult() { }

    /// <summary>
    /// Gets the data of a page.
    /// </summary>
    /// <value>
    /// The data of a page. 
    /// Its default value is never a null value.
    /// </value>
    public IEnumerable<T> Data { get; init; } = Enumerable.Empty<T>();

    /// <summary>
    /// Gets information about the page.
    /// </summary>
    /// <value>
    /// The information about the page.
    /// Its default value is <c>null</c>.
    /// </value>
    public PagedInfo PagedInfo { get; init; }

    /// <summary>
    /// Converts an instance of type <see cref="Result"/> to <see cref="PagedResult{T}"/>.
    /// </summary>
    /// <param name="result">An instance of type <see cref="Result"/>.</param>
    public static implicit operator PagedResult<T>(Result result)
        => result.ToPagedResult(Enumerable.Empty<T>(), default);


    public static implicit operator ActionResult(PagedResult<T> result)
    {
        var objectResult = new ObjectResult(result)
        {
            StatusCode = result.Status switch
            {
                ResultStatus.Ok => (int)HttpStatusCode.OK,
                ResultStatus.Created => (int)HttpStatusCode.Created,
                ResultStatus.Unauthorized => (int)HttpStatusCode.Unauthorized,
                ResultStatus.Forbidden => (int)HttpStatusCode.Forbidden,
                ResultStatus.NotFound => (int)HttpStatusCode.BadRequest,
                ResultStatus.Conflict => (int)HttpStatusCode.Conflict,
                ResultStatus.CriticalError => (int)HttpStatusCode.InternalServerError,
                ResultStatus.ByteArrayFile => (int)HttpStatusCode.OK,
                _ => (int)HttpStatusCode.BadRequest
            }
        };
        return objectResult;
    }
}
