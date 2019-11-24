using Eric.DotNetCore.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Eric.DotNetCore.Interception.AspNetCore
{
    internal class InterceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public InterceptionMiddleware(RequestDelegate next)
        {
            Guard.ArgumentNotNull(next, "next");
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            Guard.ArgumentNotNull(context, "context");
            if (null == context.RequestServices)
            {
                throw new InvalidOperationException("The service provider is not intialized.");
            }
            var original = context.RequestServices;
            try
            {
                context.RequestServices = context.RequestServices.ToInterceptable();
                await _next(context);
            }
            finally
            {
                context.RequestServices = original;
            }
        }
    }
}