using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using Fusillade;
using Refit;
using System.Net.Http;
using ModernHttpClient;
using Core.Utils;
using Akavache;
using Plugin.Connectivity;
using Polly;
using System.Net;
using System.Diagnostics;

namespace Core
{
    public class ApiService<Y, T, K>  where T : class where Y : class where K : class
    {
        #region attributes

        public const String DefaultPath = "http://fce15282.ngrok.io";
        private String Path;
        private String BasePath;
        private Priority Priority;
        private Int32 RetryCount;
        private Int32 SleepSeconds;
        private IApiService<T, K> Client;
        public Y ClientOutro;
        private Task<List<T>> TaskList;
        private Task<Y> Task;

        #endregion

        #region constructors

        public ApiService()
        {
            this.Priority = Priority.UserInitiated;
            this.RetryCount = 2;
            this.SleepSeconds = 2;
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

        public virtual Task<List<T>> FindAll()
        {
            //Func<Task<List<Y>>> myfunc = () => this.ForLazyGeneric().FindAll();
            return this
                .SetPriority(Priority.UserInitiated)
                .SetRetryCount(2)
                .SetSleepSeconds(2)
                .GetClient()
                .SetTaskList(this.Client.FindAll())
                .ExecuteList();
        }

        public virtual void FindAll(Action<Task<List<T>>> action)
        {
            this.FindAll().ContinueWith(action, TaskScheduler.FromCurrentSynchronizationContext());
        }

        public virtual void FindAll(Action<List<T>> success, Action<Exception> error)
        {
            this.FindAll().ContinueWith((task) =>
            {
                if (task.IsCompleted && task.Exception == null)
                {
                    success(task.Result);
                }
                else
                {
                    error(task.Exception);
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        public virtual Task<Y> FindOne(K key)
        {
            return this.ForLazyGeneric().FindOne(key); //this.Retry(this.ForLazyGeneric().FindOne(key));
        }

        //public Task<Y> Create([Body(BodySerializationMethod.Json)] Y payload)
        //{
        //    return this.ForGeneric().Create(payload);
        //}

        //public Task Delete(K key)
        //{
        //    return this.ForGeneric().Delete(key);
        //}

        //public void FindAll(Action<List<Y>> success, Action<Exception> error)
        //{
        //    this.FindAllCacheCallback(this.FindAll(2, false), null, success, error);
        //}

        //public Task<List<Y>> FindAll()
        //{
        //    return this.FindAll(false);
        //}

        //public Task<List<Y>> FindAll(Boolean cache = false)
        //{
        //    return this.FindAll(null, cache);
        //}
        //public Task<List<Y>> FindAll(Int32? retryCount, Boolean cache = false)
        //{
        //    Task<List<Y>> task = this.ForLazyGeneric().Value.FindAll();
        //    this.ExecuteRetryList(task, retryCount);

        //    if(cache)
        //    {
        //        task = this.FindAllCacheAsync(task, null);
        //    }
        //    return task;
        //}

        //public Task<List<Y>> FindAllPagination(int offset, int limit)
        //{
        //    return this.ForGeneric().FindAllPagination(offset, limit);
        //}

        //public Task<Y> First(K key)
        //{
        //    return this.ForGeneric().First(key);
        //}

        //public Task Update(K key, [Body(BodySerializationMethod.Json)] Y payload)
        //{
        //    return this.ForGeneric().Update(key, payload);
        //}

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

        public ApiService<Y, T, K> GetClient()
        {
            var t = new Lazy<IApiService<T, K>>(() => RestService.For<IApiService<T, K>>(this.GetLazyHttpClient(this.Priority)));
            this.Client = t.Value;
            var y = new Lazy<Y>(() => RestService.For<Y>(this.GetLazyHttpClient(this.Priority)));
            this.ClientOutro = y.Value;
            return this;
        }
        
        protected IApiService<Y, K> ForLazyGeneric(Priority priority = Priority.UserInitiated)
        {
            var t = new Lazy<IApiService<Y, K>>(() => RestService.For<IApiService<Y, K>>(this.GetLazyHttpClient(priority)));
            return t.Value;
        }

        protected T ForLazy(Priority priority = Priority.UserInitiated)
        {
            var t = new Lazy<T>(() => RestService.For<T>(this.GetLazyHttpClient(priority)));
            return t.Value;
        }

        public ApiService<Y, T, K> SetPriority(Priority priority)
        {
            this.Priority = priority;
            return this;
        }

        public ApiService<Y, T, K> SetRetryCount(Int32 retryCount)
        {
            this.RetryCount = retryCount;
            return this;
        }

        public ApiService<Y, T, K> SetSleepSeconds(Int32 sleepSeconds)
        {
            this.SleepSeconds = sleepSeconds;
            return this;
        }

        public ApiService<Y, T, K> SetTaskList(Task<List<T>> taskList)
        {
            this.TaskList = taskList;
            return this;
        }

        //protected void FirstCacheCallback(K key, DateTime? expiration, Action<Y> callackSuccess, Action<Exception> callbackError)
        //{
        //    this.FirstCacheCallback(key, this.First(key), expiration, callackSuccess, callbackError);
        //}

        //protected void FirstCacheCallback(K key, Task<Y> task, DateTime? expiration, Action<Y> callackSuccess, Action<Exception> callbackError)
        //{
        //    this.FirstCache(key, task, expiration).Subscribe(callackSuccess, callbackError);
        //}

        //protected Task<Y> FirstCacheAsync(K key, Task<Y> task, DateTime? expiration)
        //{
        //   return this.FirstCache(key, task, expiration).ToTask<Y>();
        //}

        //protected IObservable<Y> FirstCache(K key, Task<Y> task, DateTime? expiration)
        //{
        //    if (!expiration.HasValue)
        //    {
        //        expiration = DateTime.Now.AddMinutes(10);
        //    }

        //    var cache = BlobCache.LocalMachine;
        //    var cachedConferences = cache.GetAndFetchLatest(typeof(Y).Name.ToLower(), () => task, null, expiration);

        //    return cachedConferences.FirstOrDefaultAsync();
        //}

        //protected void FindAllPaginationCacheCallback(int offset, int limit, DateTime? expiration, Action<List<Y>> callackSuccess, Action<Exception> callbackError)
        //{
        //    this.FindAllCacheCallback(this.FindAllPagination(offset, limit), expiration, callackSuccess, callbackError);
        //}

        //protected void FindAllCacheCallback(DateTime? expiration, Action<List<Y>> callackSuccess, Action<Exception> callbackError)
        //{
        //    this.FindAllCacheCallback(this.FindAll(), expiration, callackSuccess, callbackError);
        //}

        //protected void FindAllCacheCallback(Task<List<Y>> task, DateTime? expiration, Action<List<Y>> callackSuccess, Action<Exception> callbackError)
        //{
        //    this.FindAllCache(task, expiration).Subscribe(callackSuccess, callbackError);
        //}

        //protected Task<List<Y>> FindAllCacheAsync(Task<List<Y>> task, DateTime? expiration)
        //{
        //    return this.FindAllCache(task, expiration).ToTask<List<Y>>();
        //}

        //protected IObservable<List<Y>> FindAllCacheObservableAsync(Task<List<Y>> task, DateTime? expiration)
        //{
        //    return this.FindAllCache(task, expiration);
        //}

        //protected IObservable<List<Y>> FindAllCache(Task<List<Y>> task, DateTime? expiration)
        //{
        //    if (!expiration.HasValue)
        //    {
        //        expiration = DateTime.Now.AddMinutes(10);
        //    }

        //    var cache = BlobCache.LocalMachine;
        //    Debug.WriteLine(typeof(Y).Name.ToLower());
        //    var cachedConferences = cache.GetAndFetchLatest(typeof(Y).Name.ToLower(), async () => await task, null, expiration);

        //    return cachedConferences;
        //}


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

        public Task<List<Y>> Retry<Y>(Func<Task<List<Y>>> taskAsync)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                var taskAux = Policy
                    .Handle<Exception>()
                    .WaitAndRetryAsync
                    (
                        retryCount: this.RetryCount,
                        sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(this.SleepSeconds, retryAttempt))
                    ).ExecuteAsync(async () => await taskAsync());

                return taskAux;
            }

            return taskAsync();
        }

        public Task<List<T>> ExecuteList()
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                var taskAux = Policy
                    .Handle<Exception>()
                    .WaitAndRetryAsync
                    (
                        retryCount: this.RetryCount,
                        sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(this.SleepSeconds, retryAttempt))
                    ).ExecuteAsync(async () => await this.TaskList);

                return taskAux;
            }

            return this.TaskList;
        }

        #endregion
    }
}
