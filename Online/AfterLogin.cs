using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Newtonsoft.Json;
using OnlineFridge.DataAccess.Model;

namespace OnlineFridge.Online
{
    [Activity(Label = "Twoja lodówka", Theme = "@style/CustomTheme")]
    public class AfterLogin : Activity
    {
       
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
            //POBRANIE DANYCH AKTUALNEGO UŻYTKOWNIKA
            var json = Intent.GetStringExtra("SerializedUser");
            
            var actualUser = JsonConvert.DeserializeObject<User>(json);

            Intent.RemoveExtra("SerializedUser");

            SetContentView(Resource.Layout.AfterLogin);

            Button btnZawartosc = FindViewById<Button>(Resource.Id.fridgeContent);
            Button btnDodawanie = FindViewById<Button>(Resource.Id.addProduct);
            Button btnKonto = FindViewById<Button>(Resource.Id.editAccount);
            Button btnWyloguj = FindViewById<Button>(Resource.Id.logout);

            btnZawartosc.Click += (x, z) =>
            {               
                var activity = new Intent(this, typeof(OnlineFridgeContent));

                activity.PutExtra("userZawartosc", json);

                if (json != null)
                {
                    StartActivity(activity);
                }
            };

            btnDodawanie.Click += (x, z) =>
            {
                var activity2 = new Intent(this, typeof(AddProductOnline));

                activity2.PutExtra("userDodawanie", json);
               
                StartActivity(activity2);
            };

            btnKonto.Click += (x, z) =>
            {
                var activity3 = new Intent(this, typeof(EditAccount));

               activity3.PutExtra("userKonto", json);

                StartActivity(activity3);
            };

            btnWyloguj.Click += (x, z) =>
            {
                var activity = new Intent(this, typeof(MainActivity));

                activity.SetFlags(ActivityFlags.NewTask);
                activity.SetFlags(ActivityFlags.ClearTask);

                StartActivity(activity);
            };
        }

        public override void OnBackPressed()
        {
            Toast.MakeText(this, "Aby wyjść naciśnij 'WYLOGUJ'", ToastLength.Short).Show();
        }
    }
}