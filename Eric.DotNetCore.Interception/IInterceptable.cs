namespace Eric.DotNetCore.Interception
{
    /// <summary>
    /// Provides a proxy which can be intercepted.
    /// </summary>
    /// <typeparam name="T">The type of the proxy provided.</typeparam>
    public interface IInterceptable<T> where T : class
    {
        /// <summary>
        /// The interceptable proxy.
        /// </summary>
        T Proxy { get; }
    }
}