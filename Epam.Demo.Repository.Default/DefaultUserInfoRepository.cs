using Epam.Demo.Core;
using Epam.Demo.DataAccess;
using System;
using System.Collections.Generic;

namespace Epam.Demo.Repository.Default
{
    public class DefaultUserInfoRepository : IUserInfoRepository
    {
        public Dictionary<string, IDataAccessor<UserInfo>> DataAccessores { get; }
        public DefaultUserInfoRepository(Dictionary<string, IDataAccessor<UserInfo>> dataAccessores)
        {
            DataAccessores = dataAccessores;
        }

        [Exception(3, "Epam.Demo.Core.ConnectionException")]
        public void Save(UserInfo userInfo)
        {
            //throw new ConnectionException();
            foreach (var item in DataAccessores)
            {
                item.Value.Add(userInfo);
            }
        }
    }
}