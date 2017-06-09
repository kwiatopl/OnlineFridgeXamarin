using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;

namespace OnlineFridge.Offline
{
    [Activity(Label = "Twoja lodówka",Theme="@style/CustomTheme2")]
    public class OfflineMode : Activity
    {
       
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.OfflineMode);

            Button btnZawartosc = FindViewById<Button>(Resource.Id.button1);
            Button btnDodawanie = FindViewById<Button>(Resource.Id.button2);
            Button btnSync = FindViewById<Button>(Resource.Id.button3);

            btnZawartosc.Click += (x, z) =>
            {
                var activity = new Intent(this, typeof(FridgeContent));
                StartActivity(activity);
            };
            
            btnDodawanie.Click += (x, z) =>
            {
                var activity = new Intent(this, typeof(AddProduct));
                StartActivity(activity);
            };

            btnSync.Click += (x, z) =>
            {
                var activity = new Intent(this, typeof(Synchronize));
                StartActivity(activity);
            };
        }
    }
}