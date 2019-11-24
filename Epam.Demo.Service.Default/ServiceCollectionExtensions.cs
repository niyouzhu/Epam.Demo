using Epam.Demo.Service;
using Epam.Demo.Service.Default;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDefaultService(this IServiceCollection services)
        {
            return services.AddSingleton<IUserService, DefaultUserService>();
        }
    }
}