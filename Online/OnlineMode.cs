using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Newtonsoft.Json;
using OnlineFridge;
using test.DataAccess.Model;

namespace test
{
    [Activity(Label = "OnlineMode",Theme = "@style/CustomTheme", NoHistory = true)]
    public class OnlineMode : Activity
    {
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
                    if (!String.IsNullOrEmpty(username.Text.ToString()) && !String.IsNullOrEmpty(pass.Text.ToString()))
                    {
                        var login = username.Text.ToString();
                        var password = pass.Text.ToString();

                        var userToLog = await GetUser(login);

                        if (userToLog.email == login && userToLog.password == password)
                        {
                            Toast.MakeText(this, "Ładowanie", ToastLength.Short).Show();

                            var serialUserLogged = JsonConvert.SerializeObject(userToLog);

                            var activity = new Intent(this, typeof(AfterLogin));

                            activity.PutExtra("ActualUser", serialUserLogged);

                            StartActivity(activity);
                        }
                        else
                        {
                            badLoginOrPass.Text = "NIEPRAWIDŁOWE HASŁO LUB UŻYTKOWNIK!";
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

        // METODA POBIERAJACA UZYTKOWNIKA PO POLU EMAIL
        public async Task<User> GetUser(string email)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://192.168.0.102:61913/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var result = await client.GetAsync(String.Format("/api/User?email={0}", email));

                return JsonConvert.DeserializeObject<User>(await result.Content.ReadAsStringAsync());       // DESERIALIZACJA OBIEKTU Z FORMATU JSON NA OBIEKT KLASY USER
            }
        }
    }
}