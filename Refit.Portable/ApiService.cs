using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fusillade;
using Refit;

namespace Refit.Portable
{
	public class ApiService<T, Y, K> : IApiService<Y, K> where T : class where Y : class where K : class
	{
		public const String DefaultPath = "http://a384b288.ngrok.io";
		private String Path;
		private String BasePath;

		public ApiService()
		{
			Path = DefaultPath;
		}

		public ApiService(String basePath) : this()
		{
			BasePath = basePath;
		}

		public ApiService(String path, String basePath)
		{
			BasePath = basePath;
			Path = path;
		}

		public Task<Y> Create([Body(BodySerializationMethod.Json)] Y payload)
		{
			return this.ForGeneric().Create(payload);
		}

		public Task Delete(K key)
		{
			return this.ForGeneric().Delete(key);
		}

		public Task<List<Y>> FindAll()
		{
			return this.ForGeneric().FindAll();
		}

		public Task<Y> FindOne(K key)
		{
			return this.ForGeneric().FindOne(key);
		}

		public Task Update(K key, [Body(BodySerializationMethod.Json)] Y payload)
		{
			return this.ForGeneric().Update(key, payload);
		}

		public IApiService<Y, K> ForGeneric()
		{
			var y = RestService.For<IApiService<Y, K>>(String.Format("{0}{1}", Path, BasePath));
			return y;
		}

		public Lazy<Y> GetPriority(Priority priority)
		{

		}

		protected T For()
		{
			var t = RestService.For<T>(String.Format("{0}{1}", Path, BasePath));
			return t;
		}
	}
}
