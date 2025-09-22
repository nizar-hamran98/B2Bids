using FluentValidation;
using MediatorCoordinator;
using Microsoft.Extensions.DependencyInjection;
using Persistence;
using SharedKernel;
using Stores.Infrastructure;
using static Stores.Domain.AppSettingsBase;

namespace Stores.Application;
public static class StoresCollectionExtensions
{
    public static IServiceCollection AddStoreModuleCore(this IServiceCollection services)
    {
        //services.AddAppSettingsConfiguration();
        services.AddSharedRegistrationsWithoutPermissions(typeof(StoresCollectionExtensions).Assembly);

        services.RegisterPersistence();

        services.AddValidatorsFromAssembly(typeof(StoresCollectionExtensions).Assembly);
        services.AddMediatorCoordinator([typeof(StoresCollectionExtensions).Assembly]);

        services.AddDbContext<StoreDbContext>();
        return services;
    }
    public static void AddAppSettingsConfiguration(this IServiceCollection services)
    {
        services.AddOptions<Authentication>()
            .BindConfiguration(Authentication.ConfigurationSection)
            .ValidateDataAnnotations()
            .ValidateOnStart();
    }
}
