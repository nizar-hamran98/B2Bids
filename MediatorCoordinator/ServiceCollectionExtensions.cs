﻿using MediatorCoordinator.Contract;
using MediatorCoordinator.PipelineBehavior;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace MediatorCoordinator
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMediatorCoordinator(this IServiceCollection services, Assembly[] assemblies)
        {
            services.AddMediatorServices(assemblies);
            services.AddPipelineBehaviors(assemblies);
            services.AddScoped<IServiceTransactionCoordinator, ServiceTransactionCoordinator>();
            return services;
        }

        private static void AddMediatorServices(this IServiceCollection services, Assembly[] assemblies)
        {
            foreach (Assembly assembly in assemblies)
            {
                var handlerTypes = assembly.GetTypes()
                   .Where(type => type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>)))
                   .ToList();

                foreach (var handlerType in handlerTypes)
                {
                    var interfaces = handlerType.GetInterfaces();
                    var requestType = interfaces.First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>)).GetGenericArguments()[0];
                    var responseType = interfaces.First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>)).GetGenericArguments()[1];

                    var serviceType = typeof(IRequestHandler<,>).MakeGenericType(requestType, responseType);
                    services.AddScoped(serviceType, handlerType);
                }
            }
        }

        private static IServiceCollection AddPipelineBehaviors(this IServiceCollection services, Assembly[] assemblies)
        {
            return services.AddMediatR(cg =>
            {
                cg.RegisterServicesFromAssemblies(assemblies);
                cg.AddOpenBehavior(typeof(RequestLoggingPipelineBehavior<,>));
                cg.AddOpenBehavior(typeof(ValidationPipelineBehavior<,>));
               // cg.AddOpenBehavior(typeof(QueryCachingPipelineBehavior<,>));
               // cg.AddOpenBehavior(typeof(CommandCachingPipelineBehavior<,>));
                cg.AddOpenBehavior(typeof(RequestTransactionPipelineBehavior<,>));
            });
        }
    }
}
