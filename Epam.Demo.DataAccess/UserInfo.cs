using System;
using System.ComponentModel.DataAnnotations;

namespace Epam.Demo.DataAccess
{
    public class UserInfo
    {
        [Key]
        public Guid Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}