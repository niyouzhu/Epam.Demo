using Microsoft.EntityFrameworkCore;

namespace Epam.Demo.DataAccess.Ef
{
    public class UserDbContext : DbContext
    {
        public DbSet<UserInfo> UserInfos { get; set; }

        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}