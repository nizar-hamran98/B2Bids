using Microsoft.Extensions.DependencyInjection;
using SharedKernel;

namespace DatabaseConnections;
public static class DBCollectionExtensions
{
    public static IServiceCollection AddB2BidsModuleCore(this IServiceCollection services)
    {
        services.AddDbContext<B2BidsDbContext>();
        return services;
    }
}
