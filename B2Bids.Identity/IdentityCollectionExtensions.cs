using FluentValidation;
using Identity.Infrastructure;
using MediatorCoordinator;
using Microsoft.Extensions.DependencyInjection;
using Persistence;
using SharedKernel;
using static Identity.Domain.AppSettingsBase;

namespace Identity.Application;
public static class IdentityCollectionExtensions
{
    public static IServiceCollection AddIdentityModuleCore(this IServiceCollection services)
    {
        services.AddAppSettingsConfiguration();
        services.AddSharedRegistrations(typeof(IdentityCollectionExtensions).Assembly);
                
        services.RegisterPersistence();

        services.AddValidatorsFromAssembly(typeof(IdentityCollectionExtensions).Assembly);
        services.AddMediatorCoordinator([typeof(IdentityCollectionExtensions).Assembly]);  

        //services.AddDbContext<IdentityDbContext>();
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

