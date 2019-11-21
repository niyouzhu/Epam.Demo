using Microsoft.EntityFrameworkCore;

namespace Epam.Demo.DataAccess.Ef
{
    public class UserDbContextCreator
    {
        public DbContextOptions<UserDbContext> DbContextOptions { get; }

        public UserDbContextCreator(DbContextOptions<UserDbContext> options)
        {
            DbContextOptions = options;
        }

        public UserDbContext Create()
        {
            return new UserDbContext(DbContextOptions);
        }
    }
}