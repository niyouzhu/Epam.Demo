using System;

namespace Eric.DotNetCore.Interception
{
    /// <summary>
    /// A factory to create an interceptable proxy.
    /// </summary>
    public interface IProxyFactory
    {
        /// <summary>
        /// Create an interceptable proxy to wrap the specified target object.
        /// </summary>
        /// <param name="typeToProxy">The return type.</param>
        /// <param name="target">The target object wrapped by the created proxy.</param>
        /// <returns>The interceptable proxy.</returns>
        object CreateProxy(Type typeToProxy, object target);
    }
}