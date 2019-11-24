using Eric.DotNetCore.Core;
using Eric.DotNetCore.Interception;
using System;

namespace Epam.Demo.Core
{
    public class ExceptionAttribute : InterceptorAttribute
    {
        public int RetrieTimes { get; set; }

        public string ExceptionName { get; set; }

        public ExceptionAttribute(int retriesTimes, string exceptionName)
        {
            Guard.ArgumentNotNullOrWhiteSpace(exceptionName, nameof(exceptionName));
            RetrieTimes = retriesTimes;
            ExceptionName = exceptionName;
        }

        public override void Use(IInterceptorBuilder interceptorBuilder)
        {
            Guard.ArgumentNotNull(interceptorBuilder, nameof(interceptorBuilder));
            interceptorBuilder.Use<ExceptionInterceptor>(this.Order, ExceptionName, RetrieTimes);
        }
    }
}