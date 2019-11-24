using Eric.DotNetCore.Core;
using Eric.DotNetCore.Interception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Defines some extension methods specific to <see cref="IServiceCollection"/>.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add interception based service registrations.
        /// </summary>
        /// <param name="services">The service collection in which the service registrations are added.</param>
        /// <param name="proxyFactoryAccessor">A delegate to get proxy factory.</param>
        /// <returns>The current service collection with interception based service registrations.</returns>
        /// <exception cref="ArgumentNullException">The specified <paramref name="services"/> is null.</exception>
        /// <exception cref="ArgumentNullException">The specified <paramref name="proxyFactoryAccessor"/> is null.</exception>
        public static IServiceCollection AddInterception(this IServiceCollection services, Func<IServiceProvider, IProxyFactory> proxyFactoryAccessor)
        {
            Guard.ArgumentNotNull(services, "services");
            Guard.ArgumentNotNull(proxyFactoryAccessor, "proxyFactoryAccessor");

            return services
                .AddScoped<IInterceptorBuilder, IInterceptorBuilder>()
                .AddScoped<IProxyFactory>(proxyFactoryAccessor)
                .AddScoped(typeof(IInterceptable<>), typeof(Interceptable<>));
        }

        /// <summary>
        /// Add interception based service registrations.
        /// </summary>
        /// <typeparam name="TProxyFactory">The type of proxy factory.</typeparam>
        /// <param name="services">The service collection in which the service registrations are added.</param>
        /// <returns>The current service collection with interception based service registrations.</returns>
        /// <exception cref="ArgumentNullException">The specified <paramref name="services"/> is null.</exception>
        public static IServiceCollection AddInterception<TProxyFactory>(this IServiceCollection services)
             where TProxyFactory : class, IProxyFactory
        {
            Guard.ArgumentNotNull(services, "services");
            return services
                .AddScoped<IInterceptorBuilder, InterceptorBuilder>()
                .AddScoped<IProxyFactory, TProxyFactory>()
                .AddScoped(typeof(IInterceptable<>), typeof(Interceptable<>));
        }

        /// <summary>
        /// Add interception features to service registrations.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <exception cref="ArgumentNullException">The specified <paramref name="services"/> is null.</exception>
        /// <returns>The original service collection.</returns>
        public static IServiceCollection ToInterceptable(this IServiceCollection services)
        {
            Guard.ArgumentNotNull(services, "services");
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            IServiceCollection newServices = GetInterceptableServices(serviceProvider, services);
            //for (int i = 0; i < services.Count; i++)
            //{
            //    services[i] = newServices[i];
            //}
            return newServices;
        }

        private static IServiceCollection GetInterceptableServices(IServiceProvider serviceProvider, IServiceCollection services)
        {
            var serviceCollection = new ServiceCollection();
            foreach (var service in services)
            {
                bool isOpneGeneric = service.ServiceType.GetTypeInfo().IsGenericTypeDefinition;
                Func<IServiceProvider, object> factory = _ => Wrap(serviceProvider, service.ServiceType);
                switch (service.Lifetime)
                {
                    case ServiceLifetime.Scoped:
                        {
                            if (isOpneGeneric)
                            {
                                serviceCollection.AddScoped(service.ServiceType, service.ImplementationType);
                            }
                            else
                            {
                                serviceCollection.AddScoped(service.ServiceType, factory);
                            }
                            break;
                        }
                    case ServiceLifetime.Singleton:
                        {
                            if (isOpneGeneric)
                            {
                                serviceCollection.AddSingleton(service.ServiceType, service.ImplementationType);
                            }
                            else
                            {
                                serviceCollection.AddSingleton(service.ServiceType, factory);
                            }
                            break;
                        }
                    case ServiceLifetime.Transient:
                        {
                            if (isOpneGeneric)
                            {
                                serviceCollection.AddTransient(service.ServiceType, service.ImplementationType);
                            }
                            else
                            {
                                serviceCollection.AddTransient(service.ServiceType, factory);
                            }
                            break;
                        }
                }
            }

            foreach (var group in serviceCollection.GroupBy(_ => _.ServiceType))
            {
                if (group.Count() > 1)
                {
                    serviceCollection.AddTransient(typeof(IEnumerable<>).MakeGenericType(group.Key),
                        _ =>
                        {
                            IProxyFactory proxyFactory = serviceProvider.GetRequiredService<IProxyFactory>();
                            var proxies = serviceProvider.GetServices(group.Key).Select(target => proxyFactory.CreateProxy(group.Key, target)).ToArray();
                            Array array = Array.CreateInstance(group.Key, proxies.Length);
                            for (int i = 0; i < group.Count(); i++)
                            {
                                array.SetValue(proxies[i], i);
                            }
                            return array;
                        });
                }
            }
            return serviceCollection;
        }

        private static object Wrap(IServiceProvider serviceProvider, Type serviceType)
        {
            object target = serviceProvider.GetService(serviceType);
            IProxyFactory proxyFactory = serviceProvider.GetRequiredService<IProxyFactory>();
            return proxyFactory.CreateProxy(serviceType, target);
        }
    }
}