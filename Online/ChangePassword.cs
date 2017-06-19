using System;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
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
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.ChangePassword);

            string regString = @"^.*(?=.{8,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[\W_]).*$";
            Regex correctPass = new Regex(regString);          
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
                if (HashAndSalt.Verify(actualUser.salt, actualUser.passwordHash, actualPass.Text.ToString()))
                {
                    if (newPass.Text.ToString() == newPassConf.Text.ToString())
                    {
                        if (correctPass.IsMatch(newPass.Text.ToString()))
                        {
                            var hashsalt = new HashAndSalt(newPass.Text.ToString());

                            if (hashsalt.Hash != actualUser.passwordHash)
                            {
                                //WARUNEK SPEŁNIONY
                                actualUser.password = newPass.Text.ToString();
                                actualUser.passwordHash = hashsalt.Hash;
                                actualUser.salt = hashsalt.Salt;

                                Toast.MakeText(this, "Ładowanie...", ToastLength.Short).Show();
                                //EWENTUALNY HASH
                                Update(actualUser);

                                Thread.Sleep(2000);
                                    Toast.MakeText(this, "Zmieniono hasło!", ToastLength.Short).Show();

                                    var activity = new Intent(this, typeof(ChangePassword));
									activity.PutExtra("userChangePass",json);
                                    StartActivity(activity);
                                
                            }
                            else
                            {
                                error.Text = "Nie można zmienić hasła na to samo!";
                            }
                        }
                        else
                        {
                            error.Text =
                                "Hasło musi mieć długość co najmniej 8 znaków oraz musi zawierać: jedną małą litere, jedną dużą litere, jedną cyfre oraz jeden symbol!";
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
                client.BaseAddress = new Uri("http://192.168.0.101:61913/");

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