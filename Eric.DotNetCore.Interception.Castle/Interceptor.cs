using Castle.DynamicProxy;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Eric.DotNetCore.Interception.Castle
{
    internal class Interceptor : IInterceptor
    {
        private InterceptorDelegate _interceptor;
        private IServiceProvider _serviceProvider;

        public Interceptor(InterceptorDelegate interceptor, IServiceProvider serviceProvider)
        {
            _interceptor = interceptor;
            _serviceProvider = serviceProvider;
        }

        public void Intercept(IInvocation invocation)
        {
            CastleInvocationContext invocationContext = new CastleInvocationContext(invocation, _serviceProvider);
            InterceptDelegate next = context => ((CastleInvocationContext)context).ProceedAsync();
            try
            {
                _interceptor(next)(invocationContext).Wait();
            }
            catch (AggregateException ex)
            {
                throw ex?.InnerException ?? ex;
            }
            catch
            {
                throw;
            }
        }
    }

    internal class CastleInvocationContext : InvocationContext
    {
        private IInvocation _invocation;

        public CastleInvocationContext(IInvocation invocation, IServiceProvider serviceProvider)
        {
            _invocation = invocation;
            this.ServiceProvider = serviceProvider;
        }

        public override IServiceProvider ServiceProvider { get; }

        public override object[] Arguments
        {
            get { return _invocation.Arguments; }
        }

        public override Type[] GenericArguments
        {
            get { return _invocation.GenericArguments; }
        }

        public override object InvocationTarget
        {
            get { return _invocation.InvocationTarget; }
        }

        public override MethodInfo Method
        {
            get { return _invocation.Method; }
        }

        public override MethodInfo MethodInvocationTarget
        {
            get { return _invocation.MethodInvocationTarget; }
        }

        public override object Proxy
        {
            get { return _invocation.Proxy; }
        }

        public override object ReturnValue
        {
            get { return _invocation.ReturnValue; }
            set { _invocation.ReturnValue = value; }
        }

        public override Type TargetType
        {
            get { return _invocation.TargetType; }
        }

        public override Task<object> GetArgumentValueAsync(int index)
        {
            return Task.FromResult(_invocation.GetArgumentValue(index));
        }

        public async Task ProceedAsync()
        {
            await Task.Run(() => _invocation.Proceed());
            await ((this.ReturnValue as Task) ?? Task.CompletedTask);
        }

        public override Task SetArgumentValueAsync(int index, object value)
        {
            _invocation.SetArgumentValue(index, value);
            return Task.CompletedTask;
        }
    }
}