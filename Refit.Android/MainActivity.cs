using Android.App;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using Refit.Portable;
using Refit;
using System.Threading.Tasks;

namespace Refit.Android
{
	[Activity(Label = "Refit.Android", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : Activity
	{
		int count = 1;

		async protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.Main);

			// Get our button from the layout resource,
			// and attach an event to it
			Button button = FindViewById<Button>(Resource.Id.myButton);

			button.Click += delegate { button.Text = $"{count++} clicks!"; };

			List<User> users = await new UserService().FindAllTeste(1, "desc");
			User user = await new UserService().FindOne("fdba6f63-99e6-4b3d-bcb4-d84719eb461a");
		}
	}
}

