using Fusillade;
using Plugin.Connectivity;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utils
{
    public static class UtilExtension
    {
       public static Func<Task<List<Y>>> TaskToFunc<Y>(this Task<List<Y>> task)
        {
            Func<Task<List<Y>>> myfunc = () => task;
            return myfunc;
        }

        public static Task<List<Y>> Execute<Y, T>(this Func<Task<List<Y>>> taskAsync, int sleepSecond = 2, int retryCount = 5)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                var taskAux = Policy
                    .Handle<Exception>()
                    .RetryAsync(retryCount)
                    //.WaitAndRetryAsync
                    //(
                    //    retryCount: retryCount,
                    //    sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(sleepSecond, retryAttempt))
                    //)
                    .ExecuteAsync(async () => await taskAsync());

                return taskAux;
            }

            return taskAsync();
        }
    }
}
