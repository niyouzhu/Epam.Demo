using Eric.DotNetCore.Core;
using Eric.DotNetCore.Interception;
using Eric.DotNetCore.Interception.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Microsoft.AspNetCore.Hosting
{
    /// <summary>
    /// Define extension to register interception based services.
    /// </summary>
    public static class WebHostBuilderExtensions
    {
        /// <summary>
        /// Add interception based services.
        /// </summary>
        /// <typeparam name="TProxyFactory">The type of proxy factory.</typeparam>
        /// <param name="builder">The <see cref="IWebHostBuilder"/> the services are registered to.</param>
        /// <returns>The <see cref="IWebHostBuilder"/> with inerception service registration.</returns>
        public static IWebHostBuilder UseInterception<TProxyFactory>(this IWebHostBuilder builder)
            where TProxyFactory : class, IProxyFactory
        {
            Guard.ArgumentNotNull(builder, "builder");
            return builder.ConfigureServices(services => services
             .AddInterception<TProxyFactory>()
             .AddTransient<IStartupFilter, InterceptionStartupFilter>());
        }
    }

    internal class InterceptionStartupFilter : IStartupFilter
    {
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return app =>
            {
                app.UseMiddleware<InterceptionMiddleware>();
                next(app);
            };
        }
    }
}