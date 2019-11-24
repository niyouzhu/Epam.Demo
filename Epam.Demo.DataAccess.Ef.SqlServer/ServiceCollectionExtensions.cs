using Epam.Demo.DataAccess.Ef;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Microsoft.Extensions.DependencyInjection

{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSqlServer<TContext>(this IServiceCollection services, Action<DbContextOptionsBuilder> builder) where TContext : DbContext
        {
            var optionsBuilder = new DbContextOptionsBuilder<UserDbContext>();
            builder(optionsBuilder);
            services.AddDbContext<TContext>(builder)
                .TryAddSingleton(typeof(UserDbContextCreator), provider => new UserDbContextCreator(optionsBuilder.Options));
            return services;
        }
    }
}