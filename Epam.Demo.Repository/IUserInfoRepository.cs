using Epam.Demo.DataAccess;

namespace Epam.Demo.Repository
{
    public interface IUserInfoRepository
    {
        void Save(UserInfo userInfo);
    }
}