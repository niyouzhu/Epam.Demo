using Epam.Demo.Repository;

namespace Epam.Demo.Business.Default
{
    public class DefaultUserInfoBusiness : IUserInfoBusiness
    {
        public IUserInfoRepository UserInfoRepository { get; }

        public DefaultUserInfoBusiness(IUserInfoRepository userInfoRepository)
        {
            UserInfoRepository = userInfoRepository;
        }

        public void Save(UserInfoBusinessObject userInfo)
        {
            UserInfoRepository.Save(userInfo.ConvertTo());
        }
    }
}