using Kernel.ApiKey;
using Kernel.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace SharedKernel;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizePermissionAttribute : Attribute, IAuthorizationFilter, IFilterMetadata
{
    private List<string> allowedPermissions { get; }

    private string errorMessage { get; } = "Forbidden";
    private const string ApiKeyHeaderName = "X-API-Key";

    public AuthorizePermissionAttribute()
    {
    }

    public AuthorizePermissionAttribute(params string[] permissions)
    {
        if (permissions.IsNotNullOrEmpty())
        {
            allowedPermissions = permissions.Where((x) => x.IsNotNullOrEmpty()).Distinct().ToList();
        }
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var apiKeyValidator = context.HttpContext.RequestServices.GetService<IApiKeyValidator>();
        string apiKey = context.HttpContext.Request.Headers[ApiKeyHeaderName];
        if (apiKey != null && apiKeyValidator.IsValid(apiKey))
            return;

        if (!context.HttpContext.User.Identity!.IsAuthenticated)
        {
            context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
        }
        else if (allowedPermissions.IsNotNullOrEmpty())
        {
            //If admin allow always
            IntranetUser user = context.GetUser();
            if ((user.IsNull() || user.Permissions.IsNullOrEmpty() || !user.Permissions.Any(x => allowedPermissions.Contains(x))))// && !user.UserName.EqualsIgnoreCase("admin"))
            {
                context.Result = new JsonResult(new { message = "you are not authorized to access this API!" }) { StatusCode = StatusCodes.Status403Forbidden };
            }
        }
    }
}
