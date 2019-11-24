using Eric.DotNetCore.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Eric.DotNetCore.Interception
{
    /// <summary>
    /// Represents the generator to generate interceptable proxy.
    /// </summary>
    public abstract class ProxyFactory : IProxyFactory
    {
        /// <summary>
        /// The service provider to provide neccessary services.
        /// </summary>
        public IServiceProvider ServiceProvider { get; }

        /// <summary>
        /// The builder to build interceptor chain.
        /// </summary>
        public IInterceptorBuilder InterceptorBuilder { get; }

        /// <summary>
        /// Create a new <see cref="ProxyFactory"/>.
        /// </summary>
        /// <param name="serviceProvider">The service provider to provide neccessary services.</param>
        /// <param name="interceptorBuilder"></param>
        /// <exception cref="ArgumentNullException">The specified <paramref name="serviceProvider"/> is null.</exception>
        /// <exception cref="ArgumentNullException">The specified <paramref name="interceptorBuilder"/> is null.</exception>
        public ProxyFactory(IServiceProvider serviceProvider, IInterceptorBuilder interceptorBuilder)
        {
            Guard.ArgumentNotNull(serviceProvider, "serviceProvider");
            Guard.ArgumentNotNull(interceptorBuilder, "interceptorBuilder");

            this.ServiceProvider = serviceProvider;
            this.InterceptorBuilder = interceptorBuilder;
        }

        /// <summary>
        /// Create the interceptable proxy based on specified type, target and method specific interceptors.
        /// </summary>
        /// <param name="typeToProxy">The type of interceptable proxy.</param>
        /// <param name="target">The target object wrapped by proxy.</param>
        /// <returns>The created interceptable proxy.</returns>
        /// <exception cref="ArgumentNullException">The specified <paramref name="typeToProxy"/> is null.</exception>
        public virtual object CreateProxy(Type typeToProxy, object target)
        {
            Guard.ArgumentNotNull(typeToProxy, "typeToProxy");
            if (!this.WillWrapTarget(target))
            {
                return target;
            }

            Dictionary<MethodInfo, InterceptorDelegate> interceptors = new Dictionary<MethodInfo, InterceptorDelegate>();
            TypeInfo targetType = target.GetType().GetTypeInfo();
            var providersForClass = GetProvidersForClass(target);
            foreach (var methodOfProxy in typeToProxy.GetTypeInfo().GetMethods().Where(it => it.DeclaringType == typeToProxy))
            {
                var providers = this.GetProvidersForMethod(methodOfProxy, target.GetType(), providersForClass);
                if (providers.Any())
                {
                    IInterceptorBuilder newBuilder = this.InterceptorBuilder.New();
                    providers.ForEach(it => it.Use(newBuilder));
                    interceptors[methodOfProxy] = newBuilder.Build();
                }
            }

            return this.Wrap(typeToProxy, target, interceptors);
        }

        /// <summary>
        /// Create interceptable proxy bying wrapping the target object based on method specific interceptors.
        /// </summary>
        /// <param name="typeToProxy">The type of interceptable proxy.</param>
        /// <param name="target">The target object wrapped by proxy.</param>
        /// <param name="interceptors">The method specific interceptors.</param>
        /// <returns>The interceptable proxy.</returns>
        protected abstract object Wrap(Type typeToProxy, object target, IDictionary<MethodInfo, InterceptorDelegate> interceptors);

        /// <summary>
        /// Determine whether to wrap the specified target.
        /// </summary>
        /// <param name="target">The target objec to wrap.</param>
        /// <returns>A <see cref="bool"/> value indicating whether to wrap the specified target object.</returns>
        protected virtual bool WillWrapTarget(object target)
        {
            if (target == null)
            {
                return false;
            }
            return target.GetType().IsInterceptable();
        }

        internal IEnumerable<IInterceptorProvider> GetProvidersForClass(object target)
        {
            var providers = target.GetType().GetTypeInfo().GetCustomAttributes().OfType<IInterceptorProvider>();
            foreach (var provider in providers)
            {
                IAttributeCollector collector = provider as IAttributeCollector;
                if (null != collector)
                {
                    collector.Attributes = target.GetType().GetTypeInfo().GetCustomAttributes();
                }
            }
            return providers;
        }

        internal IEnumerable<IInterceptorProvider> GetProvidersForMethod(MethodInfo proxyMethod, Type targetType, IEnumerable<IInterceptorProvider> providersForClass)
        {
            MethodInfo targetMethod = targetType.GetTypeInfo().GetMethods().FirstOrDefault(it => it.Matches(proxyMethod));
            if (null == targetMethod)
            {
                return new IInterceptorProvider[0];
            }
            NonInterceptableAttribute nonInterceptableAttribute = targetMethod.GetCustomAttribute<NonInterceptableAttribute>() ?? targetType.GetTypeInfo().GetCustomAttribute<NonInterceptableAttribute>();
            if (null != nonInterceptableAttribute)
            {
                return new IInterceptorProvider[0];
            }

            var providers = targetMethod.GetCustomAttributes().OfType<IInterceptorProvider>().ToList();
            //Set Attributes
            foreach (var provider in providers)
            {
                IAttributeCollector collector = provider as IAttributeCollector;
                if (null != collector)
                {
                    List<Attribute> attributes = new List<Attribute>();
                    attributes.AddRange(targetMethod.GetCustomAttributes());
                    foreach (var attribute in targetType.GetTypeInfo().GetCustomAttributes())
                    {
                        if (!attribute.AllowMultiple() && attributes.Any(it => it.GetType() == attribute.GetType()))
                        {
                            continue;
                        }
                        attributes.Add(attribute);
                    }
                    collector.Attributes = attributes;
                }
            }
            foreach (var provider in providersForClass)
            {
                if (!provider.AllowMultiple && providers.Any(it => it.GetType() == provider.GetType()))
                {
                    continue;
                }
                providers.Add(provider);
            }
            return providers;
        }
    }
}