using Epam.Demo.Business;

namespace Epam.Demo.Service
{
    public static class Extensions
    {
        public static UserInfo ConvertFrom(this UserInfoBusinessObject userInfo)
        {
            return new UserInfo() { GivenName = userInfo.FirstName, Id = userInfo.Id, Surname = userInfo.LastName };
        }
    }
}