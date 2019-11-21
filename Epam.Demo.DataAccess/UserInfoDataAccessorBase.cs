namespace Epam.Demo.DataAccess
{
    public abstract class UserInfoDataAccessorBase : IDataAccessor<UserInfo>
    {
        public abstract UserInfo Add(UserInfo entity);
    }
}