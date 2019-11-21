using Epam.Demo.DataAccess;

namespace Epam.Demo.Business
{
    public static class Extensions
    {
        public static UserInfoBusinessObject ConvertFrom(this UserInfo userInfo)
        {
            return new UserInfoBusinessObject() { FirstName = userInfo.FirstName, FullName = $"{userInfo.FirstName} {userInfo.LastName}", Id = userInfo.ToString(), LastName = userInfo.LastName };
        }
    }
}