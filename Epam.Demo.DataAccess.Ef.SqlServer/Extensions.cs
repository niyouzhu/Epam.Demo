using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Epam.Demo.DataAccess.Ef.SqlServer
{
    public static class Extensions
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