using Akavache;
using Plugin.Connectivity;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Bases
{
    public class BaseController<T>
    {
        protected Task<List<T>> GetRemoteListAsync(Task<List<T>> remoteTask)
        {
            Task<List<T>> task = new Task<List<T>>(() => new List<T>() );
            if (CrossConnectivity.Current.IsConnected)
            {
                task = Policy
                      .Handle<WebException>()
                      .WaitAndRetryAsync
                      (
                        retryCount: 5,
                        sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                      )
                      .ExecuteAsync(async () => await remoteTask);
            }
            return task;
        }

        protected void GetListFromCache(string key, Task<List<T>> remoteTask, Action<List<T>> callbackSuccess, DateTime? expirateDate = null)
        {
            if(!expirateDate.HasValue)
            {
                expirateDate = DateTime.Now.AddMinutes(10);
            }
            BlobCache.LocalMachine.GetAndFetchLatest<List<T>>(key,
                async () => await remoteTask,
                null,
                expirateDate
            ).Catch(Observable.Return(new List<T>())).Subscribe(callbackSuccess);
        }
    }
}
