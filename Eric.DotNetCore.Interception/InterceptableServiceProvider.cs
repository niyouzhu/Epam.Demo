using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Eric.DotNetCore.Interception
{
    internal class InterceptableServiceProvider : IServiceProvider
    {
        private readonly IServiceProvider _innerProvider;
        private readonly IProxyFactory _proxyFactory;

        public InterceptableServiceProvider(IServiceProvider innerProvider)
        {
            _innerProvider = innerProvider;
            _proxyFactory = innerProvider.GetRequiredService<IProxyFactory>();
        }

        public object GetService(Type serviceType)
        {
            var service = _innerProvider.GetService(serviceType);
            if (serviceType.GetTypeInfo().IsGenericType)
            {
                Type genericTypeDefintion = serviceType.GetGenericTypeDefinition();
                if (genericTypeDefintion == typeof(IEnumerable<>))
                {
                    IEnumerable enumerable = (IEnumerable)service;
                    List<object> list = new List<object>();
                    foreach (var it in enumerable)
                    {
                        list.Add(it);
                    }
                    Type elementType = serviceType.GenericTypeArguments[0];
                    Array array = Array.CreateInstance(elementType, list.Count);
                    for (int i = 0; i < list.Count; i++)
                    {
                        var proxy = _proxyFactory.CreateProxy(elementType, list[i]);
                        array.SetValue(proxy, i);
                    }
                    return array;
                }
            }
            return _proxyFactory.CreateProxy(serviceType, service);
        }
    }
}