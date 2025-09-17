using Microsoft.Extensions.Configuration;

namespace Kernel.ApiKey;
public class ApiKeyValidator(IConfiguration configuration) : IApiKeyValidator
{
    public bool IsValid(string apiKey)
    {
        var validApiKey = configuration["RedisConfiguration:ApiKey:Value"];
        return apiKey.Equals(validApiKey);
    }
}

public interface IApiKeyValidator
{
    bool IsValid(string apiKey);
}