using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using test.DataAccess.Model;

namespace OnlineFridge.Online
{
    [Activity(Label = "Zmiana hasła", Theme="@style/CustomTheme", NoHistory = true)]
    public class ChangePassword : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.ChangePassword);

            var json = Intent.GetStringExtra("SerializedUser");
            var actualUser = JsonConvert.DeserializeObject<User>(json);

            //INICJOWANIE WIDOKOW
            EditText actualPass = FindViewById<EditText>(Resource.Id.actualPass);
            EditText newPass = FindViewById<EditText>(Resource.Id.newPass);
            EditText newPassConf = FindViewById<EditText>(Resource.Id.confPass);
            TextView error = FindViewById<TextView>(Resource.Id.error);

            //INICJOWANIE PRZYCISKÓW
            Button buttonChange = FindViewById<Button>(Resource.Id.buttonChangePass);

            buttonChange.Click += (x, z) =>
            {
                if (actualPass.Text.ToString() == actualUser.password)
                {
                    if (newPass.Text.ToString() == newPassConf.Text.ToString())
                    {
                        if (newPass.Text.ToString() != actualUser.password)
                        {
                            //WARUNEK SPEŁNIONY
                            actualUser.password = newPass.Text.ToString();
                            actualUser.passwordHash = newPass.Text.ToString();
                            //EWENTUALNY HASH

                            Update(actualUser);

                            Toast.MakeText(this, "Zmieniono hasło!", ToastLength.Short).Show();

                            var activity = new Intent(this, typeof(ChangePassword));
                            StartActivity(activity);
                        }
                        else
                        {
                            error.Text = "Nie można zmienić hasła na to samo!";
                        }
                    }
                    else
                    {
                        error.Text = "Podane hasła się różnią!";
                    }
                }               
                else
                {
                    error.Text = "Błędne aktualne hasło!";
                }
            };

        }

        private async void Update(User userToPut)
        {
            await UpdateUser(userToPut);
        }

        public async Task UpdateUser(User user)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://192.168.0.102:61913/");

                var json = JsonConvert.SerializeObject(user);

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                await client.PostAsync("/api/User/{" + user.userId + "}", content);
            }
        }

        protected override void OnStop()
        {
            base.OnStop();
            Finish();
        }
    }
}