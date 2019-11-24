using Eric.DotNetCore.Core;
using Eric.DotNetCore.Interception;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Epam.Demo.Core
{
    public class ExceptionInterceptor
    {
        private InterceptDelegate _next;
        private string _exceptionName;
        private int _retriedTimes;


        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionInterceptor"/> class.
        /// </summary>
        /// <param name="next">The next <see cref="InterceptDelegate"/>.</param>
        /// <param name="exceptionName">The name of the exception.</param>
        /// <param name="retriedTimes">The retried times if occurs connection exception.</param>
        public ExceptionInterceptor(InterceptDelegate next, string exceptionName, int retriedTimes)
        {
            Guard.ArgumentNotNull(next, nameof(next));
            _retriedTimes = retriedTimes;
            _exceptionName = exceptionName;
            _next = next;

        }

        /// <summary>
        /// The method will be invoke when intercepting.
        /// </summary>
        /// <param name="context">The <see cref="InvocationContext"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task InvokeAsync(InvocationContext context)
        {
            Guard.ArgumentNotNull(context, nameof(context));
            for (int i = 0; i < _retriedTimes + 1; i++)
            {
                try
                {
                    await _next(context);
                    break;
                }
                catch (Exception ex)
                {
                    if (ex.GetType().FullName == _exceptionName)
                    {
                        if (i == _retriedTimes - 1)
                            throw;
                    }
                    else
                        throw;
                }
            }
        }
    }
}
