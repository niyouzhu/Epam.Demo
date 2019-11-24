using System;
using System.Collections.Generic;
using System.Reflection;

namespace Eric.DotNetCore.Interception
{
    internal static class AttributeExtensions
    {
        private static Dictionary<Type, bool> _indicators = new Dictionary<Type, bool>();
        private static object _syncHelper = new object();

        public static bool AllowMultiple(this Attribute attribute)
        {
            bool allow;
            if (_indicators.TryGetValue(attribute.GetType(), out allow))
            {
                return allow;
            }
            lock (_syncHelper)
            {
                if (_indicators.TryGetValue(attribute.GetType(), out allow))
                {
                    return allow;
                }
                AttributeUsageAttribute usage = attribute.GetType().GetTypeInfo().GetCustomAttribute<AttributeUsageAttribute>();
                return _indicators[attribute.GetType()] = usage?.AllowMultiple == true;
            }
        }
    }
}