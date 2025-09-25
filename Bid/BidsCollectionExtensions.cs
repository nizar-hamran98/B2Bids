using Bid.Infrastructure;
using FluentValidation;
using MediatorCoordinator;
using Microsoft.Extensions.DependencyInjection;
using Persistence;
using SharedKernel;
using static Bid.Domain.AppSettingsBase;

namespace Bid.Application;
public static class BidsCollectionExtensions
{
    public static IServiceCollection AddBidModuleCore(this IServiceCollection services)
    {
        services.AddAppSettingsConfiguration();
        services.AddSharedRegistrationsWithoutPermissions(typeof(BidsCollectionExtensions).Assembly);

        services.RegisterPersistence();

        services.AddValidatorsFromAssembly(typeof(BidsCollectionExtensions).Assembly);
        services.AddMediatorCoordinator([typeof(BidsCollectionExtensions).Assembly]);

        //services.AddDbContext<BidDbContext>();
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
