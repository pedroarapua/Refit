using System;
using Akavache;
using Android.App;
using Android.Runtime;
namespace Droid
{
    [Application]
    public class Initialize : Application
    {
        public Initialize(IntPtr handle, JniHandleOwnership ownerShip) : base(handle, ownerShip)
        {
        }
        public override void OnCreate()
        {
            base.OnCreate();
            BlobCache.ApplicationName = "MyApp";
        }
    }
}
