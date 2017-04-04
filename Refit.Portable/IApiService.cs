using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Refit;

namespace Refit.Portable
{
	public interface IApiService<T, in TKey> where T : class
	{
		[Post("")]
		Task<T> Create([Body] T payload);

		[Get("")]
		Task<List<T>> FindAll();

		[Get("/{key}")]
		Task<T> FindOne(TKey key);

		[Put("/{key}")]
		Task Update(TKey key, [Body]T payload);

		[Delete("/{key}")]
		Task Delete(TKey key);
	}
}
