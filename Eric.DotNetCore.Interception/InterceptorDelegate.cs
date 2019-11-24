using System.Threading.Tasks;

namespace Eric.DotNetCore.Interception
{
    /// <summary>
    /// A delegate representing to perform interception functionality in the <see cref="InvocationContext"/>.
    /// </summary>
    /// <param name="context">The <see cref="InvocationContext"/> representing the method call against the interceptable proxy.</param>
    /// <returns>The task to perform interception functionality.</returns>
    public delegate Task InterceptDelegate(InvocationContext context);

    /// <summary>
    /// A delegate representing an interceptor.
    /// </summary>
    /// <param name="next">The <see cref="InterceptDelegate"/> to call the next interceptor or target object.</param>
    /// <returns>A <see cref="InterceptDelegate"/> to perform interception functionality.</returns>
    public delegate InterceptDelegate InterceptorDelegate(InterceptDelegate next);
}