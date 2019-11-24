using Eric.DotNetCore.Core;
using Eric.DotNetCore.Interception;

namespace System
{
    /// <summary>
    /// Define some extension methods specific to <see cref="IServiceProvider"/>.
    /// </summary>
    public static class ServiceProviderExtensions
    {
        /// <summary>
        /// Convert the specific service provider to a new one which can provide interceptable proxy warpping the target servce instance.
        /// </summary>
        /// <param name="serviceProvider">The service provider to convert.</param>
        /// <returns>The interceptable proxy warpping the target servce instance.</returns>
        /// <exception cref="ArgumentNullException">The argument <paramref name="serviceProvider"/> is null.</exception>
        public static IServiceProvider ToInterceptable(this IServiceProvider serviceProvider)
        {
            Guard.ArgumentNotNull(serviceProvider, nameof(serviceProvider));
            return new InterceptableServiceProvider(serviceProvider);
        }
    }
}