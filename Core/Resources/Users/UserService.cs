using System;
using System.Collections.Generic;
using System.Net;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Akavache;
using Fusillade;
using Plugin.Connectivity;
using Core.Utils;
using Polly;
using Refit;

namespace Core
{
    //public class UserService : ApiService<IUserService, User, string>, IUserService
    public class UserService : ApiService<IUserService, User, string>, IUserService
    {
        public UserService() : base("/users")
        {

        }

        public Task<List<User>> FindAllOutro()
        {
            return this
               .SetPriority(Priority.UserInitiated)
               .SetRetryCount(2)
               .SetSleepSeconds(2)
               .GetClient()
               .SetTaskList(this.ClientOutro.FindAllOutro())
               .ExecuteList();
        }

        //public Task<List<User>> FindAll()
        //{
        //    return base.FindAll(2, true);
        //return await base.ForLazy().Value.FindAllTeste(1, "desc");

        //if (CrossConnectivity.Current.IsConnected)
        //         {
        //             users = await Policy
        //                 .Handle<Exception>()
        //                 .RetryAsync(retryCount: 5)
        //                 .ExecuteAsync(async () => await task);
        //         }



        //}
    }
}
