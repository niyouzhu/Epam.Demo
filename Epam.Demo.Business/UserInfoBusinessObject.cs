using Epam.Demo.DataAccess;
using System;

namespace Epam.Demo.Business
{
    public class UserInfoBusinessObject
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Id { get; set; }

        public string FullName { get; set; }

        public UserInfo ConvertTo()
        {
            return new UserInfo() { Id = string.IsNullOrWhiteSpace(Id) ? Guid.NewGuid() : Guid.Parse(Id), LastName = LastName, FirstName = FirstName };
        }
    }
}