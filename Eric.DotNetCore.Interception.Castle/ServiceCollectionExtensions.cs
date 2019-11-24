using Eric.DotNetCore.Interception.Castle;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Define extension methods to register <see cref="DynamicProxyFactory"/> related services.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Register <see cref="DynamicProxyFactory"/> related services.
        /// </summary>
        /// <param name="services">The service collection in which the <see cref="DynamicProxyFactory"/> is registered.</param>
        /// <returns>The current service collection.</returns>
        /// <exception cref="ArgumentNullException">The specified <paramref name="services"/> is null.</exception>
        public static IServiceCollection AddCastleInterception(this IServiceCollection services)
        {
            return services.AddInterception<DynamicProxyFactory>();
        }
    }
}