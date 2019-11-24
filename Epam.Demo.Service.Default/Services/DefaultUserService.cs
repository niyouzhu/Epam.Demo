using Epam.Demo.Business;
using Epam.Demo.Core;
using Grpc.Core;
using System.Threading.Tasks;

namespace Epam.Demo.Service.Default
{
    public class DefaultUserService : User.UserBase, IUserService
    {
        public IUserInfoBusiness UserInfoBusiness { get; }

        public DefaultUserService(IUserInfoBusiness userInfoBusiness)
        {
            UserInfoBusiness = userInfoBusiness;
        }

        public override Task<UserReply> SaveInfo(UserRequest request, ServerCallContext context)
        {
            Save(request.ConvertTo());
            return Task.FromResult(new UserReply { Status = "OK" });
        }
        public void Save(UserInfo user)
        {
            UserInfoBusiness.Save(user.ConvertTo());
        }
    }
}