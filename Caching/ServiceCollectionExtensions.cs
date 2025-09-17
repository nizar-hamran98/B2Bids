using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SharedKernel;
using StackExchange.Redis;

namespace Caching;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDistributedCache(this IServiceCollection services)
    {
        services.AddOptions<RedisConfiguration>()
            .BindConfiguration(RedisConfiguration.ConfigurationSection)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddSingleton<IConfigurationApiClient>(provider =>
        {
            var options = provider.GetRequiredService<IOptions<RedisConfiguration>>().Value;
            return new ConfigurationApiClient(options);
        });

        services.AddSingleton((Func<IServiceProvider, IConnectionMultiplexer>)(provider =>
        {
            var options = provider.GetRequiredService<IOptions<RedisConfiguration>>().Value;
            var configurationOptions = ConfigurationOptions.Parse(options.RedisURL);

            if (!string.IsNullOrEmpty(options.AccessToken))
                configurationOptions.Password = options.AccessToken;

            return ConnectionMultiplexer.Connect(configurationOptions);
        })).AddSingleton<IRedisHash, RedisHash>();

        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        return services;
    }
}
