using Epam.Demo.Business;

namespace Epam.Demo.Service
{
    public class UserInfo
    {
        public string Id { get; set; }

        public string Surname { get; set; }

        public string GivenName { get; set; }

        public UserInfoBusinessObject ConvertTo()
        {
            return new UserInfoBusinessObject() { FirstName = GivenName, FullName = $"{GivenName} {Surname}", Id = Id, LastName = Surname };
        }
    }
}