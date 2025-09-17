using Microsoft.AspNetCore.Mvc;

namespace Kernel.ApiKey;

public class ApiKeyAttribute : ServiceFilterAttribute
{
    public ApiKeyAttribute()
        : base(typeof(ApiKeyAuthorizationFilter))
    {
    }
}
