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
			return base.For().FindAllTeste(1, "desc");
		}
	}
}
