using Microsoft.Extensions.DependencyInjection;

namespace Epam.Demo.Business.Default
{
    public static class Extensions
    {
        public static IServiceCollection AddDefaultBusiness(this IServiceCollection services)
        {
            return services.AddSingleton<IUserInfoBusiness, DefaultUserInfoBusiness>();
        }
    }
}