namespace Epam.Demo.DataAccess.Ef
{
    public class UserInfoDataAccessor : UserInfoDataAccessorBase
    {
        public UserDbContextCreator UserDbContextCreator { get; }

        public UserInfoDataAccessor(UserDbContextCreator userDbContextCreator)
        {
            UserDbContextCreator = userDbContextCreator;
        }

        public override UserInfo Add(UserInfo userInfo)
        {
            using (var dbContext = UserDbContextCreator.Create())
            {
                var entity = dbContext.Add(userInfo);
                dbContext.SaveChanges();
                return entity.Entity;
            }
        }
    }
}