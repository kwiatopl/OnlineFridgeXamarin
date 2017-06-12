using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Newtonsoft.Json;
using OnlineFridge.DataAccess.Model;

namespace OnlineFridge.Online
{
    [Activity(Label = "Register", Theme = "@style/CustomTheme", NoHistory = true)]
    public class Register : Activity
    {
        private bool flag;
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Register);

            //INICJOWANIE WIDOKOW
            EditText email = FindViewById<EditText>(Resource.Id.email);
            EditText pass = FindViewById<EditText>(Resource.Id.pass);
            TextView error = FindViewById<TextView>(Resource.Id.error);

            //INICJOWANIE PRZYCISKÓW
            Button buttonRegister = FindViewById<Button>(Resource.Id.buttonRegister);

            string regString = @"^.*(?=.{8,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[\W_]).*$";
            Regex correctPass = new Regex(regString);

            buttonRegister.Click += async (sender, e) =>
            {
                flag = false;
                var passwordToRegister = pass.Text.ToString();
                var emailToRegister = email.Text.ToString();

                if (!String.IsNullOrWhiteSpace(passwordToRegister) || String.IsNullOrWhiteSpace(emailToRegister))
                {
                    if (IsValidLogin(emailToRegister))
                    {
                        // WYWOŁANIE METODY GetUser
                        // POBRANIE UŻYTKOWNIKA O TAKIM SAMYM EMAILU JAK WPISANY
                        Toast.MakeText(this, "Ładowanie...", ToastLength.Short).Show();

                        var userToCheckIfExist = await GetUser(emailToRegister);

                        
                            if (userToCheckIfExist == null) // SPRAWDZENIE CZY TAKI UŻYTKOWNIK ISTNIEJE
                            {
                                if (correctPass.IsMatch(passwordToRegister))
                                {
                                    var hashsalt = new HashAndSalt(passwordToRegister);

                                    var u = new User();
                                    u.email = emailToRegister;
                                    u.password = passwordToRegister;
                                    u.passwordHash = hashsalt.Hash;
                                    u.salt = hashsalt.Salt;
                                    u.username = emailToRegister;

                                    PostUser(u); // DODANIE UŻYTKOWNIKA

                                    Toast.MakeText(this, "Zarejestrowano!", ToastLength.Short).Show();
                                }
                                else
                                {
                                    error.Text =
                                        "Hasło musi mieć długość co najmniej 8 znaków oraz musi zawierać: jedną małą litere, jedną dużą litere, jedną cyfre oraz jeden symbol!";
                                }
                            }
                            else
                            {
                                Toast.MakeText(this, "Taki użytkownik już istnieje!", ToastLength.Short).Show();
                            }
                        }
                        else
                        {
                            error.Text = "Nieprawidłowy format email!";
                        }
                    }
                    else
                    {
                        Toast.MakeText(this, "Uzupełnij pola!", ToastLength.Short).Show();
                    }
                
            };
        }

        // METODA POBIERAJACA UZYTKOWNIKA PO POLU EMAIL
        public async Task<User> GetUser(string email)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://192.168.0.103:61913/");
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
                    Toast.MakeText(this,"Wystąpił błąd!",ToastLength.Short).Show();
                    return null;
                }
            }
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

        public async void PostUser(User user)
        {
            await AddUser(user);
        }

        // METODA DODAJACA UZYTKOWNIKA DO BAZY DANYCH
        public async Task AddUser(User user)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://192.168.0.103:61913/");

                var json = JsonConvert.SerializeObject(user);       // SERIALIZACJA OBIEKTU NA FORMAT JSON

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("/api/User", content);
            }
        }
        public override void OnBackPressed()
        {
            flag = true;
            var activity = new Intent(this, typeof(OnlineMode));
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