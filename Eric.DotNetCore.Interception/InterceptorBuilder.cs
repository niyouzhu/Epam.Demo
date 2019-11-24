using Eric.DotNetCore.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Eric.DotNetCore.Interception
{
    /// <summary>
    /// The default implementation of interceptor builder.
    /// </summary>
    public class InterceptorBuilder : IInterceptorBuilder
    {
        private List<Tuple<int, InterceptorDelegate>> _interceptors = new List<Tuple<int, InterceptorDelegate>>();

        /// <summary>
        /// The service provider to provide neccessary services.
        /// </summary>
        public IServiceProvider ServiceProvider { get; }

        /// <summary>
        /// Create a new <see cref="InterceptorBuilder"/>.
        /// </summary>
        /// <param name="serviceProvider">The service provider to provide neccessary services.</param>
        /// <exception cref="ArgumentNullException">The specified <paramref name="serviceProvider"/>.</exception>
        public InterceptorBuilder(IServiceProvider serviceProvider)
        {
            Guard.ArgumentNotNull(serviceProvider, "serviceProvider");
            this.ServiceProvider = serviceProvider;
        }

        /// <summary>
        /// Collect a new interceptor.
        /// </summary>
        /// <param name="interceptor">The interceptor to collect.</param>
        /// <param name="order">The order in the interceptor chain to build.</param>
        /// <returns>The current inteceptor builder.</returns>
        /// <exception cref="ArgumentNullException">The specified <paramref name="interceptor"/> is null.</exception>
        public IInterceptorBuilder Use(InterceptorDelegate interceptor, int order)
        {
            Guard.ArgumentNotNull(interceptor, "interceptor");
            InterceptorDelegate wrapper = next => (async context =>
            {
                try
                {
                    await interceptor(next)(context);
                }
                catch (Exception ex)
                {
                    if (!context.ExceptionHandled)
                    {
                        throw new InterceptionException(Resources.ExceptionInterceptionError, ex);
                    }
                    else
                    {
                        throw ex;
                    }
                }
            });

            _interceptors.Add(new Tuple<int, InterceptorDelegate>(order, wrapper));
            return this;
        }

        /// <summary>
        /// Build a chain using collected interceptor based on their order.
        /// </summary>
        /// <returns>A <see cref="InterceptorDelegate"/> which represents the interceptor chain to build.</returns>
        public InterceptorDelegate Build()
        {
            var interceptors = from it in _interceptors
                               orderby it.Item1
                               select it.Item2;
            return next =>
            {
                var current = next;
                foreach (var it in interceptors.Reverse())
                {
                    current = it(current);
                }
                return current;
            };
        }

        /// <summary>
        /// Create a new interceptor builder.
        /// </summary>
        /// <returns>The interceptor builder to create.</returns>
        public IInterceptorBuilder New()
        {
            return new InterceptorBuilder(this.ServiceProvider);
        }
    }
}