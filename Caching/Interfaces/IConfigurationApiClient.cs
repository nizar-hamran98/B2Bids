namespace Caching;

public interface IConfigurationApiClient
{
    Task<string?> List(string controller, string apiKey, string serviceUrl);
    Task<string?> RefreshByPropertyNameAsync(string controller, string propertyName, object propertyValue, string apiKey, string serviceUrl);
}
