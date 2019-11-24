using System;

namespace Eric.DotNetCore.Interception
{
    /// <summary>
    /// An attribute indicating the target method will never be intercepted.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class NonInterceptableAttribute : Attribute
    {
    }
}