using Android.App;
using Android.Widget;
using Android.OS;
using System.Linq;
using Core;
using System.Threading.Tasks;
using Akavache;
using System;
using System.Collections.Generic;

namespace Droid
{
    [Activity(Label = "Droid", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
        int count = 1;


        protected override async void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            TextView text = FindViewById<TextView>(Resource.Id.txtView);
            Button button = FindViewById<Button>(Resource.Id.myButton);

            button.Click += delegate { button.Text = $"{count++} clicks!"; };

            var service = new UserService();
            // chamada 1
            try
            {
                var users = await service.FindAllOutro();
                text.Text = String.Format("Carregando => " + users.First().Avatar);
            }
            catch (Exception ex)
            {

            }

            // chamada 2
            //await service
            //    .FindAll()
            //    .ContinueWith((taskUsers) =>
            //    {
            //        if (taskUsers.IsCompleted && taskUsers.Exception == null)
            //        {
            //            OnSuccess(taskUsers.Result);
            //        }
            //        else
            //        {
            //            OnError(taskUsers.Exception);
            //        }
            //    }, TaskScheduler.FromCurrentSynchronizationContext());

            // chamada 3
            //service
            //    .FindAll((taskUsers) =>
            //    {
            //        if (taskUsers.IsCompleted && taskUsers.Exception == null)
            //        {
            //            OnSuccess(taskUsers.Result);
            //        }
            //        else
            //        {
            //            OnError(taskUsers.Exception);
            //        }
            //    });

            // chamada 4
            //service
            //    .FindAll(OnSuccess, OnError);
        }

        private void OnSuccess(List<User> users)
        {
            TextView text = FindViewById<TextView>(Resource.Id.txtView);
            text.Text = String.Format("Carregando => " + users.Last().Avatar);
        }

        private void OnError(Exception ex)
        {
            TextView text = FindViewById<TextView>(Resource.Id.txtView);
            text.Text = String.Format("Exception => " + ex.Message);
        }
    }
}

