using Eric.DotNetCore.Core;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace Eric.DotNetCore.Interception
{
    /// <summary>
    /// Define some extension methods specific to <see cref="IInterceptorBuilder"/>.
    /// </summary>
    public static class InterceptorBuilderExtensions
    {
        private const string InterceptMethodName = "InvokeAsync";

        internal delegate Task InvokeDelegate(object interceptor, InvocationContext context, IServiceProvider serviceProvider);

        private static Dictionary<Type, InvokeDelegate> _factories = new Dictionary<Type, InvokeDelegate>();
        private static object _syncHelper = new object();
        private static MethodInfo GetServiceMethod = typeof(InterceptorBuilderExtensions).GetTypeInfo().GetMethod("GetService", BindingFlags.Static | BindingFlags.NonPublic);

        /// <summary>
        /// Add a new interceptor to interceptor chain.
        /// </summary>
        /// <typeparam name="TInterceptor">The type of interceptor.</typeparam>
        /// <param name="builder">The interceptor builder.</param>
        /// <param name="order">The order to determine the position in the interceptor chain.</param>
        /// <param name="arguments">The arguments passed to constructor.</param>
        /// <returns>The current interceptor builder.</returns>
        /// <exception cref="ArgumentNullException">The specified <paramref name="builder"/> is null.</exception>
        /// <exception cref="ArgumentException">The specified <typeparamref name="TInterceptor"/> is not a valid interceptor.
        /// <para>The valid interceptor class should be defined as below.</para>
        /// <code>
        /// public class FoobarInterceptor
        /// {
        ///     private InterceptDelegate _next;
        ///     private IFoo              _foo;
        ///
        ///     public FoobarInterceptor(InterceptDelegate next, IFoo foo)
        ///     {
        ///         _next = next;
        ///         _foo = foo;
        ///     }
        ///
        ///     public Task InvokeAsync(InvocationContext context, IBar bar)
        ///     {
        ///         ...
        ///         await _next(context);
        ///     }
        /// }
        /// </code>
        /// </exception>
        public static IInterceptorBuilder Use<TInterceptor>(this IInterceptorBuilder builder, int order, params object[] arguments)
        {
            return builder.Use(typeof(TInterceptor), order, arguments);
        }

        /// <summary>
        /// Add a new interceptor to interceptor chain.
        /// </summary>
        /// <param name="builder">The interceptor builder.</param>
        /// <param name="interceptorType">The type of interceptor.</param>
        /// <param name="order">The order to determine the position in the interceptor chain.</param>
        /// <param name="arguments">The arguments passed to constructor.</param>
        /// <returns>The current interceptor builder.</returns>
        /// <exception cref="ArgumentNullException">The specified <paramref name="builder"/> is null.</exception>
        /// <exception cref="ArgumentNullException">The specified <paramref name="interceptorType"/> is null.</exception>
        /// <exception cref="ArgumentException">The specified <paramref name="interceptorType"/> is not a valid interceptor.
        /// <para>The valid interceptor class should be defined as below.</para>
        /// <code>
        /// public class FoobarInterceptor
        /// {
        ///     private InterceptDelegate _next;
        ///     private IFoo              _foo;
        ///
        ///     public FoobarInterceptor(InterceptDelegate next, IFoo foo)
        ///     {
        ///         _next = next;
        ///         _foo = foo;
        ///     }
        ///
        ///     public Task InvokeAsync(InvocationContext context, IBar bar)
        ///     {
        ///         ...
        ///         await _next(context);
        ///     }
        /// }
        /// </code>
        /// </exception>
        public static IInterceptorBuilder Use(this IInterceptorBuilder builder, Type interceptorType, int order, params object[] arguments)
        {
            Guard.ArgumentNotNull(builder, "builder");
            Guard.ArgumentNotNull(interceptorType, "interceptorType");
            InterceptorDelegate interceptor = next => (async context =>
            {
                object[] newArguments = new object[arguments.Length + 1];
                newArguments[0] = next;
                arguments.CopyTo(newArguments, 1);
                object instance = ActivatorUtilities.CreateInstance(builder.ServiceProvider, interceptorType, newArguments);
                InvokeDelegate factory = GetFactory(interceptorType);
                await factory(instance, context, builder.ServiceProvider);
            });
            return builder.Use(interceptor, order);
        }

        internal static InvokeDelegate GetFactory(Type interceptorType)
        {
            InvokeDelegate factory;
            if (_factories.TryGetValue(interceptorType, out factory))
            {
                return factory;
            }
            lock (_syncHelper)
            {
                if (_factories.TryGetValue(interceptorType, out factory))
                {
                    return factory;
                }

                MethodInfo interceptMethod;
                if (!TryGetInvokeMethod(interceptorType, out interceptMethod))
                {
                    throw new ArgumentException(Resources.ExceptionInvokeMethodNotExists.Fill(interceptorType.FullName));
                }

                ParameterExpression interceptor = Expression.Parameter(typeof(object), "interceptor");
                ParameterExpression invocationContext = Expression.Parameter(typeof(InvocationContext), "context");
                ParameterExpression serviceProvider = Expression.Parameter(typeof(IServiceProvider), "serviceProvider");

                var arguments = interceptMethod.GetParameters().Select(it => GetArgument(invocationContext, serviceProvider, it.ParameterType));

                Expression instance = interceptor;
                instance = Expression.Convert(instance, interceptMethod.DeclaringType);

                Expression invoke = Expression.Call(instance, interceptMethod, arguments.ToArray());
                return _factories[interceptorType] = Expression.Lambda<InvokeDelegate>(invoke, interceptor, invocationContext, serviceProvider).Compile();
            }
        }

        internal static bool TryGetInvokeMethod(Type interceptorType, out MethodInfo invokeMethod)
        {
            var result = from method in interceptorType.GetTypeInfo().GetMethods()
                         let parameters = method.GetParameters()
                         where method.Name == InterceptMethodName &&
                         parameters.FirstOrDefault()?.ParameterType == typeof(InvocationContext) &&
                         method.ReturnType == typeof(Task)
                         select method;
            return (invokeMethod = result.FirstOrDefault()) != null;
        }

        private static Expression GetArgument(Expression httpContext, Expression serviceProvider, Type parameterType)
        {
            if (parameterType == typeof(InvocationContext))
            {
                return httpContext;
            }
            Expression serviceType = Expression.Constant(parameterType, typeof(Type));
            Expression callGetService = Expression.Call(GetServiceMethod, serviceProvider, serviceType);
            return Expression.Convert(callGetService, parameterType);
        }

        private static object GetService(IServiceProvider serviceProvider, Type serviceType)
        {
            return serviceProvider.GetService(serviceType);
        }
    }
}