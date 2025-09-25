using FluentValidation;
using MediatorCoordinator;
using Microsoft.Extensions.DependencyInjection;
using Persistence;
using Products.Infrastructure;
using SharedKernel;
using static Product.Domain.AppSettingsBase;

namespace Products.Application;
public static class ProductCollectionExtensions
{
    public static IServiceCollection AddProductModuleCore(this IServiceCollection services)
    {
        services.AddAppSettingsConfiguration();
        services.AddSharedRegistrationsWithoutPermissions(typeof(ProductCollectionExtensions).Assembly);

        services.RegisterPersistence();

        services.AddValidatorsFromAssembly(typeof(ProductCollectionExtensions).Assembly);
        services.AddMediatorCoordinator([typeof(ProductCollectionExtensions).Assembly]);

        //services.AddDbContext<ProductDbContext>();
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
