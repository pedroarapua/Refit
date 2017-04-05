using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Refit;

namespace Refit.Portable
{
    [Headers("Accept: application/json")]
    public interface IUserService
	{
		[Get("/")]
		Task<List<User>> FindAllTeste([AliasAs("id")] int groupId, [AliasAs("sort")] string sortOrder);
	}
}
