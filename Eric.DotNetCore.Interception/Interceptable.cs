using Eric.DotNetCore.Core;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Eric.DotNetCore.Interception
{
    /// <summary>
    /// Provides a proxy which can be intercepted.
    /// </summary>
    /// <typeparam name="T">The type of the proxy provided.</typeparam>
    public class Interceptable<T> : IInterceptable<T> where T : class
    {
        private IServiceProvider _serviceProvider;

        /// <summary>
        /// Create a new <see cref="Interceptable&lt;Task&gt;"/> object.
        /// </summary>
        /// <param name="serviceProvider">The service providuer to provide injected service.</param>
        /// <exception cref="ArgumentNullException">The specified argument <paramref name="serviceProvider"/> is null.</exception>
        /// <exception cref="InvalidOperationException">The <paramref name="serviceProvider"/> cannot provide the <see cref="ProxyFactory"/>.</exception>
        public Interceptable(IServiceProvider serviceProvider)
        {
            Guard.ArgumentNotNull(serviceProvider, "serviceProvider");

            _serviceProvider = serviceProvider;
            T target = _serviceProvider.GetService<T>();
            if (null == target)
            {
                this.Proxy = null;
            }
            IProxyFactory proxyFactory = _serviceProvider.GetRequiredService<IProxyFactory>();
            this.Proxy = proxyFactory.CreateProxy<T>(target);
        }

        /// <summary>
        /// The interceptable proxy.
        /// </summary>
        public T Proxy { get; }
    }
}