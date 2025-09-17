using System.Net.Http.Headers;

namespace Caching;
public class ConfigurationApiClient : IConfigurationApiClient
{
    private readonly HttpClient _httpClient;
    public ConfigurationApiClient(RedisConfiguration options)
    {
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        _httpClient.DefaultRequestHeaders.Add(options.ApiKey.Key, options.ApiKey.Value);
    }

    public async Task<string?> List(string controller, string apiKey, string serviceUrl)
    {
        var response = await _httpClient.GetAsync($"{serviceUrl}/{controller}/GetFromDB");
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();

        return json;
    }

    public async Task<string?> RefreshByPropertyNameAsync(string controller, string propertyName, object propertyValue, string apiKey, string serviceUrl)
    {
        try
        {
            var response = await _httpClient.GetAsync($"{serviceUrl}/{controller}/GetFromDB?{propertyName}={propertyValue}");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return !string.IsNullOrEmpty(json) ? json : null;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex.InnerException);
        }
    }
}
