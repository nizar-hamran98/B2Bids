using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Kernel.ApiKey;
public class ApiKeyAuthorizationFilter(IApiKeyValidator apiKeyValidator) : IAuthorizationFilter
{
    private const string ApiKeyHeaderName = "X-API-Key";
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        string apiKey = context.HttpContext.Request.Headers[ApiKeyHeaderName];
        if (!apiKeyValidator.IsValid(apiKey))
            context.Result = new UnauthorizedResult();
    }
}
