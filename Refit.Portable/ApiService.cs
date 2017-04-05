using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fusillade;
using Refit;
using System.Net.Http;
using ModernHttpClient;

namespace Refit.Portable
{
	public class ApiService<T, Y, K> : IApiService<Y, K> where T : class where Y : class where K : class
	{
        #region attributes

        public const String DefaultPath = "http://dc93439f.ngrok.io";
		private String Path;
		private String BasePath;
        
        #endregion

        #region constructors

        public ApiService()
		{
        }

		public ApiService(String basePath) : this()
		{
            Path = DefaultPath;
            BasePath = basePath;
		}

		public ApiService(String path, String basePath) : this(basePath)
		{
			Path = path;
        }

        #endregion

        //#region properties

        //public T Background
        //{
        //    get { return _background.Value; }
        //}

        //public T UserInitiated
        //{
        //    get { return _userInitiated.Value; }
        //}

        //public T Speculative
        //{
        //    get { return _speculative.Value; }
        //}
        //#endregion

        #region public methods

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

        #endregion

        //public Lazy<Y> GetPriority(Priority priority)
        //{

        //}

        #region protected methods

        protected IApiService<Y, K> ForGeneric()
        {
            var y = RestService.For<IApiService<Y, K>>(this.GeDefaultHttpClient());
            return y;
        }

        protected T For()
		{
			var t = RestService.For<T>(this.GeDefaultHttpClient());			
            return t;
		}

        protected Lazy<IApiService<Y, K>> ForLazyGeneric(Priority priority = Priority.UserInitiated)
        {
            var t = new Lazy<IApiService<Y, K>>(() => RestService.For<IApiService<Y, K>>(this.GetLazyHttpClient(priority)));
            return t;
        }

        protected Lazy<T> ForLazy(Priority priority = Priority.UserInitiated)
        {
            var t = new Lazy<T>(() => RestService.For<T>(this.GetLazyHttpClient(priority)));
            return t;
        }

        #endregion

        #region private methods

        private HttpClient GeDefaultHttpClient()
        {
            return new HttpClient(new NativeMessageHandler())
            {
                BaseAddress = new Uri(String.Format("{0}{1}", Path, BasePath))
            };
        }

        private HttpClient GetLazyHttpClient(Priority priority)
        {
            return new HttpClient(new RateLimitedHttpMessageHandler(new NativeMessageHandler(), priority))
            {
                BaseAddress = new Uri(String.Format("{0}{1}", Path, BasePath))
            };
        }

        //private Lazy<IApiService<Y, K>> LazyGeneric(Priority priority = Priority.UserInitiated)
        //{
        //    var y = new Lazy<IApiService<Y, K>>(() => RestService.For<IApiService<Y, K>>(this.GetLazyHttpClient(priority)));
        //    return y;
        //}

        //private Lazy<T> Lazy(Priority priority = Priority.UserInitiated)
        //{
        //    var t = new Lazy<T>(() => RestService.For<T>(this.GetLazyHttpClient(priority)));
        //    return t;
        //}

        #endregion
    }
}
