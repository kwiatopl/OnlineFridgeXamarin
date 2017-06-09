using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using OnlineFridge.Online;
using test.DataAccess.Model;

namespace OnlineFridge
{
    [Activity(Label = "Edycja konta",Theme="@style/CustomTheme")]
    public class EditAccount : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.EditAccount);

            var json = Intent.GetStringExtra("SerializedUser");
            var actualUser = JsonConvert.DeserializeObject<User>(json);

            var btnChangePass = FindViewById<Button>(Resource.Id.passChange);
            var btnDelUser = FindViewById<Button>(Resource.Id.accountDel);

            btnChangePass.Click += (x, z) =>
            {
                var activity = new Intent(this, typeof(ChangePassword));

                activity.PutExtra("SerializedUser", json);
                StartActivity(activity);
            };

            btnDelUser.Click += (x, z) =>
            {
                
            };

        }
    }
}