using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Newtonsoft.Json;
using OnlineFridge.DataAccess.Model;
using System.Net.Mail;

namespace OnlineFridge.Online
{
    [Activity(Label = "OnlineMode",Theme = "@style/CustomTheme")]
    public class OnlineMode : Activity
    {

        public bool flag;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.OnlineMode);
            
            //INICJOWANIE WIDOKÓW
            TextView badLoginOrPass = FindViewById<TextView>(Resource.Id.badPass);

            EditText username = FindViewById<EditText>(Resource.Id.username);
            EditText pass = FindViewById<EditText>(Resource.Id.password);

            //INICJOWANIE PRZYCISKÓW
            Button btnLogin = FindViewById<Button>(Resource.Id.buttonLogin);
            Button btnRegister = FindViewById<Button>(Resource.Id.buttonRegister);

            //KLIKNIĘCIE LOGIN


            btnLogin.Click += async (sender, e) =>
            {
                flag = false;

                if (!String.IsNullOrEmpty(username.Text.ToString()) && !String.IsNullOrEmpty(pass.Text.ToString()))
                {
                    if (IsValidLogin(username.Text.ToString()))
                    {
                        var login = username.Text.ToString();
                        var password = pass.Text.ToString();

                        Toast.MakeText(this, "Ładowanie...", ToastLength.Short).Show();

                        var userToLog = await GetUser(login);

                        if (!flag)
                        {
                            if (userToLog != null)
                            {
                                if (userToLog.email == login && userToLog.password == password)
                                {
                                    var activity = new Intent(this, typeof(AfterLogin));

                                    var serialUserLogged = JsonConvert.SerializeObject(userToLog);

                                    activity.PutExtra("SerializedUser", serialUserLogged);

                                    StartActivity(activity);
                                }
                                else
                                {
                                    badLoginOrPass.Text = "NIEPRAWIDŁOWE HASŁO LUB UŻYTKOWNIK!";
                                }
                            }
                            else
                            {
                                badLoginOrPass.Text = "NIEPRAWIDŁOWE HASŁO LUB UŻYTKOWNIK!";
                            }

                        }
                    }
                    else
                    {
                        badLoginOrPass.Text = "NIEPRAWIDŁOWY FORMAT EMAIL!";
                    }
                }
                else
                {
                    Toast.MakeText(this, "Uzupełnij pola!", ToastLength.Short).Show();
                }
            };

            //KLIKNIECIE REGISTER
            btnRegister.Click += (sender, e) =>
            {
                var activity = new Intent(this, typeof(Register));
                StartActivity(activity);
            };

        }

        private bool IsValidLogin(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        // METODA POBIERAJACA UZYTKOWNIKA PO POLU EMAIL
        public async Task<User> GetUser(string email)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://192.168.1.17:61913/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var result = await client.GetAsync(String.Format("/api/User?email={0}", email));
                if (result.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<User>(await result.Content
                        .ReadAsStringAsync()); // DESERIALIZACJA OBIEKTU Z FORMATU JSON NA OBIEKT KLASY USER
                }
                else
                {
                    return null;
                }
            }
        }

        public override void OnBackPressed()
        {
            flag = true;
            var activity = new Intent(this, typeof(MainActivity));
            activity.SetFlags(ActivityFlags.NewTask);
            activity.SetFlags(ActivityFlags.ClearTask);
            StartActivity(activity);
        }

        protected override void OnStop()
        {
            base.OnStop();
            Finish();
        }
    }
}