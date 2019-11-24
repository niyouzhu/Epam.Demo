using Epam.Demo.Business;
using Epam.Demo.Business.Default;
namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDefaultBusiness(this IServiceCollection services)
        {
            return services.AddSingleton<IUserInfoBusiness, DefaultUserInfoBusiness>();
        }
    }
}