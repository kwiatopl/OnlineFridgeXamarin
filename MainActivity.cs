using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Widget;
using OnlineFridge.Offline;
using OnlineFridge.Online;
namespace OnlineFridge
{
    [Activity(Theme = "@style/CustomTheme")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView (Resource.Layout.Main);

           
            Button btnOnline = FindViewById<Button>(Resource.Id.button1);
            btnOnline.Click += (z, x) =>
            {
                var _activity = new Intent(this, typeof(OnlineMode));
                StartActivity(_activity);
            };

            Button btnOffline = FindViewById<Button>(Resource.Id.button2);
            TextView text = FindViewById<TextView>(Resource.Id.textView1);
            text.SetTextColor(Color.ParseColor("#35bfe8"));

            btnOffline.Click += (z, x) =>{
                var activity2 = new Intent(this, typeof(OfflineMode));
                StartActivity(activity2);
            };

            
        }
    }
}

