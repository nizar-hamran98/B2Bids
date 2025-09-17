using System.ComponentModel.DataAnnotations;

namespace Caching;
public class RedisConfiguration
{
    [Required]

    public const string ConfigurationSection = "RedisConfiguration";

    [Required]
    public string RedisURL { get; init; } = string.Empty;

    [Required]
    public string SourceSystemKey { get; init; } = string.Empty;
    public string AccessToken { get; init; } = string.Empty;

    public Dictionary<string, string>? BaseURLs { get; set; }
    public ApiKey ApiKey { get; set; }
}
public class ApiKey
{
    public string Key { get; set; }
    public string Value { get; set; }
}
