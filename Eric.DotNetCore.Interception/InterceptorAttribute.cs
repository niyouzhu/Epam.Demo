using System;
using System.Collections.Generic;
using System.Reflection;

namespace Eric.DotNetCore.Interception
{
    /// <summary>
    /// An <see cref="Attribute"/> to apply an interceptor to class or method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public abstract class InterceptorAttribute : Attribute, IInterceptorProvider, IAttributeCollector
    {
        private bool? _allowMultiple;

        /// <summary>
        /// A <see cref="bool"/> value indicating whether multiple interceptors can be applied to the same method.
        /// </summary>
        /// <remarks>If an interceptor attribute whose AllowMultiple is false is applied to both class and its methods, only the one applied to method takes effect.</remarks>
        public bool AllowMultiple
        {
            get
            {
                if (_allowMultiple.HasValue)
                {
                    return _allowMultiple.Value;
                }
                return (_allowMultiple = this.GetType().GetTypeInfo().GetCustomAttribute<AttributeUsageAttribute>().AllowMultiple).Value;
            }
        }

        /// <summary>
        /// Gets or sets the attributes.
        /// </summary>
        /// <value>
        /// The attributes.
        /// </value>
        public IEnumerable<Attribute> Attributes { get; set; }

        /// <summary>
        /// The order in the interceptor chain.
        /// </summary>
        /// <remarks>The the interceptor with the same order, the one applied to class level will be firt invoked.</remarks>
        public int Order { get; set; }

        /// <summary>
        /// Collect the interceptor using specified inteceptor builder.
        /// </summary>
        /// <param name="interceptorBuilder">The inteceptor builder to collect interceptor.</param>
        /// <exception cref="ArgumentNullException">The specified <paramref name="interceptorBuilder"/> is null.</exception>
        public abstract void Use(IInterceptorBuilder interceptorBuilder);
    }
}