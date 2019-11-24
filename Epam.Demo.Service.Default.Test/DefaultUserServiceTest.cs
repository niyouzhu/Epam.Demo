using Epam.Demo.Business.Default;
using Epam.Demo.DataAccess;
using Epam.Demo.DataAccess.Ef;
using Epam.Demo.Repository.Default;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Xunit;

namespace Epam.Demo.Service.Default.Test
{
    public class DefaultUserServiceTest
    {
        private string ConnectionString { get; } = @"Server=.;Database=EpamDemoTest;Trusted_Connection=True;MultipleActiveResultSets=true";

        [Fact]
        public void TestSave()
        {
            var dbContextOptionBuilder = new DbContextOptionsBuilder<UserDbContext>();
            dbContextOptionBuilder.UseSqlServer(ConnectionString);
            var accessores = new Dictionary<string, IDataAccessor<DataAccess.UserInfo>>
            {
                { "default", new UserInfoDataAccessor(new UserDbContextCreator(dbContextOptionBuilder.Options)) }
            };
            var repository = new DefaultUserInfoRepository(accessores);
            var business = new DefaultUserInfoBusiness(repository);
            var userService = new DefaultUserService(business);
            userService.Save(new UserInfo() { GivenName = "Bar", Surname = "Foo" });
        }
    }
}