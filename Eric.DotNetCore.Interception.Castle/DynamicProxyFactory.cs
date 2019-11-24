using Castle.DynamicProxy;
using Eric.DotNetCore.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Eric.DotNetCore.Interception.Castle
{
    /// <summary>
    /// A proxy generator which leverages Castle based dynamic proxy to generate the interceptabled proxy.
    /// </summary>
    public class DynamicProxyFactory : ProxyFactory
    {
        private ProxyGenerator _proxyGenerator;

        /// <summary>
        /// Create a new <see cref="DynamicProxyFactory"/>.
        /// </summary>
        /// <param name="serviceProvider">The service provider to provide neccessary services.</param>
        /// <param name="interceptorBuilder"></param>
        public DynamicProxyFactory(IServiceProvider serviceProvider, IInterceptorBuilder interceptorBuilder) : base(serviceProvider, interceptorBuilder)
        {
            _proxyGenerator = new ProxyGenerator();
        }

        /// <summary>
        /// Create the interceptable proxy based on specified type, target and method specific interceptors.
        /// </summary>
        /// <param name="typeToProxy">The type of interceptable proxy.</param>
        /// <param name="target">The target object wrapped by proxy.</param>
        /// <param name="interceptors">The method specific interceptors.</param>
        /// <returns>The created interceptable proxy.</returns>
        /// <exception cref="ArgumentNullException">The specified <paramref name="typeToProxy"/> is null.</exception>
        /// <exception cref="ArgumentNullException">The specified <paramref name="target"/> is null.</exception>
        /// <exception cref="ArgumentNullException">The specified <paramref name="interceptors"/> is null.</exception>
        protected override object Wrap(Type typeToProxy, object target, IDictionary<MethodInfo, InterceptorDelegate> interceptors)
        {
            Guard.ArgumentNotNull(typeToProxy, "typeToProxy");
            Guard.ArgumentNotNull(target, "target");
            Guard.ArgumentNotNull(interceptors, "interceptors");

            Dictionary<MemberInfo, IInterceptor> dictionary = new Dictionary<MemberInfo, IInterceptor>();
            foreach (var it in interceptors)
            {
                dictionary.Add(it.Key, new Interceptor(it.Value, this.ServiceProvider));
            }
            ProxyGenerationOptions options = new ProxyGenerationOptions()
            {
                Selector = new InterceptorSelector(dictionary)
            };

            if (typeToProxy.GetTypeInfo().IsInterface)
            {
                return _proxyGenerator.CreateInterfaceProxyWithTarget(typeToProxy, target, options, dictionary.Values.ToArray());
            }
            else
            {
                return _proxyGenerator.CreateClassProxyWithTarget(typeToProxy, target, dictionary.Values.ToArray());
            }
        }

        /// <summary>
        /// Determine whether to wrap the specified target.
        /// </summary>
        /// <param name="target">The target objec to wrap.</param>
        /// <returns>A <see cref="bool"/> value indicating whether to wrap the specified target object.</returns>
        protected override bool WillWrapTarget(object target)
        {
            if (!base.WillWrapTarget(target))
            {
                return false;
            }
            return !(target.GetType().Namespace ?? "").StartsWith("Castle.Proxies");
        }
    }
}