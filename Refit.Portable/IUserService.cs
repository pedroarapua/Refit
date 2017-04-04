using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Refit;

namespace Refit.Portable
{
	public interface IUserService
	{
		[Get("/")]
		Task<List<User>> FindAllTeste([AliasAs("id")] int groupId, [AliasAs("sort")] string sortOrder);
	}
}
