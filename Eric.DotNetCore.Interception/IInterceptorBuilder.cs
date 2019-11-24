using System;

namespace Eric.DotNetCore.Interception
{
    /// <summary>
    /// The builder to build the interceptor pipeline.
    /// </summary>
    public interface IInterceptorBuilder
    {
        /// <summary>
        /// The service provider to provide neccessary services.
        /// </summary>
        IServiceProvider ServiceProvider { get; }

        /// <summary>
        /// Collect a new interceptor.
        /// </summary>
        /// <param name="interceptor">The interceptor to collect.</param>
        /// <param name="order">The order in the interceptor chain to build.</param>
        /// <returns>The current inteceptor builder.</returns>
        IInterceptorBuilder Use(InterceptorDelegate interceptor, int order);

        /// <summary>
        /// Build a chain using collected interceptor based on their order.
        /// </summary>
        /// <returns>A <see cref="InterceptorDelegate"/> which represents the interceptor chain to build.</returns>
        InterceptorDelegate Build();

        /// <summary>
        /// Create a new interceptor builder.
        /// </summary>
        /// <returns>The interceptor builder to create.</returns>
        IInterceptorBuilder New();
    }
}