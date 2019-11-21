using Microsoft.Extensions.DependencyInjection;
using System;

namespace Epam.Demo.Service.Default
{
    public static class Extensions
    {
        public static UserInfo ConvertTo(this UserRequest userReqeust)
        {
            var splited = userReqeust.Fullname.Split(new[] { " ", "," }, StringSplitOptions.RemoveEmptyEntries);
            if (splited.Length > 1)
            {
                return new UserInfo() { GivenName = splited[0], Surname = splited.Length > 1 ? splited[1] : null };
            }
            else
            {
                return new UserInfo() { Surname = splited[0].Substring(0, 1), GivenName = splited[0].Substring(1, splited[0].Length - 1) };
            }
        }

        public static IServiceCollection AddDefaultService(this IServiceCollection services)
        {
            return services.AddSingleton<IUserService, DefaultUserService>();
        }
    }
}