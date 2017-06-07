using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace test
{
    [Activity(Label = "Splash Screen App", MainLauncher = true, Theme = "@style/Theme.Splash", NoHistory = true, Icon = "@drawable/Icon")]
    public class SplashScreen : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            //Display Splash Screen for X Sec  
            Thread.Sleep(2000);
            StartActivity(typeof(MainActivity));
        }
    }
}