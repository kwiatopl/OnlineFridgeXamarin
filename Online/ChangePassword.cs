using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Newtonsoft.Json;
using OnlineFridge.DataAccess.Model;

namespace OnlineFridge.Online
{
    [Activity(Label = "Zmiana hasła", Theme="@style/CustomTheme", NoHistory = true)]
    public class ChangePassword : Activity
    {
        private bool flag;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.ChangePassword);

            flag = false;
            var actualUser = new User();

            var json = Intent.GetStringExtra("userChangePass");
            if (json != null)
            {
                actualUser = JsonConvert.DeserializeObject<User>(json);
            }

            Intent.RemoveExtra("userChangePass");

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

                            Toast.MakeText(this, "Ładowanie...", ToastLength.Short).Show();
                            //EWENTUALNY HASH
                            Update(actualUser);

                            Thread.Sleep(2000);
                            if (flag)
                            {
                                Toast.MakeText(this, "Zmieniono hasło!", ToastLength.Short).Show();

                                var activity = new Intent(this, typeof(ChangePassword));
                                StartActivity(activity);
                            }
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
            flag = true;
        }

        public async Task UpdateUser(User user)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://192.168.1.17:61913/");

                var json = JsonConvert.SerializeObject(user);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PutAsync("/api/User", content);

                if (!response.IsSuccessStatusCode)
                {
                    Toast.MakeText(this, "Wystąpił błąd!", ToastLength.Short).Show();
                }
            }
        }

        protected override void OnStop()
        {
            base.OnStop();
            Finish();
        }
    }
}