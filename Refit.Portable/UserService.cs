using Fusillade;
using Plugin.Connectivity;
using Polly;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Refit.Portable
{
	public class UserService : ApiService<IUserService, User, string>, IUserService
	{
		public UserService() : base("/users")
		{
			
		}

		public Task<List<User>> FindAllTeste([AliasAs("id")] int groupId, [AliasAs("sort")] string sortOrder)
		{
            List<User> users = null;

            //Task<List<User>> task = base.ForLazy().Value.FindAllTeste(1, "desc");
			return base.ForLazy().Value.FindAllTeste(1, "desc");

			/*
            if (CrossConnectivity.Current.IsConnected)
            {
                users = await Policy
                    .Handle<Exception>()
                    .RetryAsync(retryCount: 5)
                    .ExecuteAsync(async () => await task);
            }


            return users;
            */
        }
	}
}
