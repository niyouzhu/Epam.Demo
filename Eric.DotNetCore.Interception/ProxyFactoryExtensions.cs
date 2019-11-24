using Eric.DotNetCore.Core;
using System;

namespace Eric.DotNetCore.Interception
{
    /// <summary>
    /// Defines some extension methods specific to <see cref="IProxyFactory"/>.
    /// </summary>
    public static class ProxyFactoryExtensions
    {
        /// <summary>
        /// Create the interceptable proxy based on specified  target object.
        /// </summary>
        /// <typeparam name="TProxy">The return type of created interceptable proxy.</typeparam>
        /// <param name="proxyFactory">The factory to create interceptable proxy.</param>
        /// <param name="target">The target object wrapped by the created proxy.</param>
        /// <returns>The created interceptable proxy.</returns>
        /// <exception cref="ArgumentNullException">The specified <paramref name="proxyFactory"/> is null.</exception>
        public static TProxy CreateProxy<TProxy>(this IProxyFactory proxyFactory, TProxy target)
        {
            Guard.ArgumentNotNull(proxyFactory, "proxyFactory");
            return (TProxy)proxyFactory.CreateProxy(typeof(TProxy), target);
        }
    }
}