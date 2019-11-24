using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Eric.DotNetCore.Interception.Castle
{
    internal class InterceptorSelector : IInterceptorSelector
    {
        private IDictionary<MemberInfo, IInterceptor> _interceptors;

        public InterceptorSelector(IDictionary<MemberInfo, IInterceptor> interceptors)
        {
            _interceptors = interceptors;
        }

        public IInterceptor[] SelectInterceptors(Type type, MethodInfo method, IInterceptor[] interceptors)
        {
            IInterceptor interceptor;
            if (_interceptors.TryGetValue(method, out interceptor) && interceptors.Contains(interceptor))
            {
                return new IInterceptor[] { interceptor };
            }
            return new IInterceptor[0];
        }
    }
}