using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Newtonsoft.Json;
using OnlineFridge.DataAccess.Model;

namespace OnlineFridge.Online
{
    [Activity(Label = "Edycja konta",Theme="@style/CustomTheme")]
    public class EditAccount : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.EditAccount);

            var json = Intent.GetStringExtra("userKonto");
            var actualUser = JsonConvert.DeserializeObject<User>(json);

            Intent.RemoveExtra("userKonto");

            var btnChangePass = FindViewById<Button>(Resource.Id.passChange);
            var btnDelUser = FindViewById<Button>(Resource.Id.accountDel);

            btnChangePass.Click += (x, z) =>
            {
                var activity = new Intent(this, typeof(ChangePassword));

                activity.PutExtra("userChangePass", json);
                StartActivity(activity);
            };

            btnDelUser.Click += (x, z) =>
            {
                // TUTAJ DODAC
            };

        }
    }
}