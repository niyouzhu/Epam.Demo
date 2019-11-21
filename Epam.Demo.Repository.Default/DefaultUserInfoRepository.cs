using Epam.Demo.Core;
using Epam.Demo.DataAccess;
using System;
using System.Collections.Generic;

namespace Epam.Demo.Repository.Default
{
    public class DefaultUserInfoRepository : IUserInfoRepository
    {
        public Dictionary<string, IDataAccessor<UserInfo>> DataAccessores { get; }
        public DefaultRepositoryOptions RepositoryOptions { get; }

        public DefaultUserInfoRepository(Dictionary<string, IDataAccessor<UserInfo>> dataAccessores, DefaultRepositoryOptions defaultRepositoryOptions)
        {
            DataAccessores = dataAccessores;
            RepositoryOptions = defaultRepositoryOptions;
        }

        /// <summary>
        /// Finally I want to realize a intecepter that can intecept the method which is marked by ExceptionAttribute.
        /// So that we can remove the try...catch, but now I haven't realized that yet, another way is add a new class named "...Proxy" that can retry 3 times if failed.
        /// but here i just use for(i=0;i<RetriedTimes;i++)
        /// </summary>
        /// <param name="userInfo"></param>
        [Exception(3, "ConnectionException")]
        public void Save(UserInfo userInfo)
        {
            foreach (var item in DataAccessores)
            {
                for (int i = 0; i < RepositoryOptions.RetriedTimes; i++)
                {
                    try
                    {
                        item.Value.Add(userInfo);
                        break;
                    }
                    catch (ConnectionException)
                    {
                        if (i == RepositoryOptions.RetriedTimes)
                        {
                            throw;
                        }
                    }
                }
            }
        }
    }
}