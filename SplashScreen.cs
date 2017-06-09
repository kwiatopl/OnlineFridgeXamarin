using System.Threading;
using Android.App;
using Android.OS;

namespace OnlineFridge
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