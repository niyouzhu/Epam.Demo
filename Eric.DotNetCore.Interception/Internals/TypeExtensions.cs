using Eric.DotNetCore.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Eric.DotNetCore.Interception
{
    internal static class TypeExtensions
    {
        private static Dictionary<Type, bool> _cached = new Dictionary<Type, bool>();
        private static object _syncHelper = new object();

        public static bool IsInterceptable(this Type type)
        {
            Guard.ArgumentNotNull(type, "type");
            bool interceptable;
            if (_cached.TryGetValue(type, out interceptable))
            {
                return interceptable;
            }

            lock (_syncHelper)
            {
                TypeInfo typeInfo = type.GetTypeInfo();
                if (typeInfo.GetCustomAttributes().OfType<NonInterceptableAttribute>().Any())
                {
                    return (_cached[type] = false);
                }
                if (typeInfo.GetCustomAttributes().OfType<InterceptorAttribute>().Any())
                {
                    return (_cached[type] = true);
                }
                if (typeInfo.GetMethods().Any(it => !it.GetCustomAttributes().OfType<NonInterceptableAttribute>().Any() && it.GetCustomAttributes().OfType<InterceptorAttribute>().Any()))
                {
                    return (_cached[type] = true);
                }
            }
            return (_cached[type] = false);
        }
    }
}