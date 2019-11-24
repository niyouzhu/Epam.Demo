using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Eric.DotNetCore.Interception
{
    /// <summary>
    ///  Encapsulates an invocation of a proxied method.
    /// </summary>
    public abstract class InvocationContext
    {
        /// <summary>
        /// Service provider to provide neccessary services.
        /// </summary>
        public abstract IServiceProvider ServiceProvider { get; }

        /// <summary>
        ///  Gets the arguments that the Castle.DynamicProxy.IInvocation.Method has been invokedwith.
        /// </summary>
        public abstract object[] Arguments { get; }

        /// <summary>
        ///  Gets the generic arguments of the method.
        /// </summary>
        public abstract Type[] GenericArguments { get; }

        /// <summary>
        ///  Gets the object on which the invocation is performed. This is different from proxy object because most of the time this will be the proxy target object.
        /// </summary>
        public abstract object InvocationTarget { get; }

        /// <summary>
        ///  Gets the System.Reflection.MethodInfo representing the method being invoked on the proxy.
        /// </summary>
        public abstract MethodInfo Method { get; }

        /// <summary>
        ///  For interface proxies, this will point to the System.Reflection.MethodInfo on the target class.
        /// </summary>
        public abstract MethodInfo MethodInvocationTarget { get; }

        /// <summary>
        ///  Gets the proxy object on which the intercepted method is invoked.
        /// </summary>
        public abstract object Proxy { get; }

        /// <summary>
        /// Gets or sets the return value of the method.
        /// </summary>
        public abstract object ReturnValue { get; set; }

        /// <summary>
        ///  Gets the type of the target object for the intercepted method.
        /// </summary>
        public abstract Type TargetType { get; }

        /// <summary>
        /// Gets the value of the argument at the specified index.
        /// </summary>
        /// <param name="index">The index of argument to get.</param>
        /// <returns>The task to get argument value.</returns>
        public abstract Task<object> GetArgumentValueAsync(int index);

        /// <summary>
        /// Overrides the value of an argument at the given index with the new value provided.
        /// </summary>
        /// <param name="index">The index of argument to set.</param>
        /// <param name="value">The value of argument to set.</param>
        /// <returns>The task to set argument.</returns>
        public abstract Task SetArgumentValueAsync(int index, object value);

        /// <summary>
        /// Gets or sets a value indicating whether the thrown exception has been handled by an interceptor.
        /// </summary>
        /// <value>
        ///   <c>true</c> if thrown exception has been handled by an interceptor; otherwise, <c>false</c>.
        /// </value>
        public bool ExceptionHandled { get; set; }
    }
}