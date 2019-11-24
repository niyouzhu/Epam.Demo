namespace Eric.DotNetCore.Interception
{
    /// <summary>
    /// Represents an interceptor provider to collect the interceptor using specified interceptor builder.
    /// </summary>
    public interface IInterceptorProvider
    {
        /// <summary>
        /// Collect the interceptor using specified inteceptor builder.
        /// </summary>
        /// <param name="interceptorBuilder">The inteceptor builder to collect interceptor.</param>
        void Use(IInterceptorBuilder interceptorBuilder);

        /// <summary>
        /// An indicator to determine if the interceptor chain can contain multiple interceptors of the same type.
        /// </summary>
        bool AllowMultiple { get; }
    }
}