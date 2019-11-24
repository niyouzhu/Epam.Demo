using Epam.Demo.DataAccess;
using Epam.Demo.DataAccess.Ef;
using Epam.Demo.Repository;
using Epam.Demo.Repository.Default;
using Eric.DotNetCore.Interception;
using System;
using System.Collections.Generic;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDefaultRepository(this IServiceCollection services)
        {
            services.AddTransient(provider =>
            {
                var accessors = new Dictionary<string, IDataAccessor<UserInfo>>();
                accessors.Add("default", new UserInfoDataAccessor(provider.GetService<UserDbContextCreator>()));
                return accessors;
            });
            services.Add(new ServiceDescriptor(typeof(IUserInfoRepository), (provider) =>
            {

                return provider.GetRequiredService<IProxyFactory>().CreateProxy(typeof(IUserInfoRepository), new DefaultUserInfoRepository(provider.GetRequiredService<Dictionary<string, IDataAccessor<UserInfo>>>()));

            }, ServiceLifetime.Singleton));
            return services;

            //return services.AddSingleton<IUserInfoRepository, DefaultUserInfoRepository>();
        }
    }
}