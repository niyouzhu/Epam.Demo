using Epam.Demo.DataAccess;
using Epam.Demo.DataAccess.Ef;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Epam.Demo.Repository.Default
{
    public static class Extensions
    {
        public static IServiceCollection AddDefaultRepository(this IServiceCollection services, Action<DefaultRepositoryOptions> options)
        {
            var defaultRepositoryOptions = new DefaultRepositoryOptions();
            options(defaultRepositoryOptions);
            services.AddTransient(provider =>
            {
                var accessors = new Dictionary<string, IDataAccessor<UserInfo>>();
                accessors.Add("default", new UserInfoDataAccessor(provider.GetService<UserDbContextCreator>()));
                return accessors;
            });

            services.AddSingleton(defaultRepositoryOptions);
            return services.AddSingleton<IUserInfoRepository, DefaultUserInfoRepository>();
        }
    }
}