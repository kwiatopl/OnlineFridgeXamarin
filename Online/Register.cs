using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.OS;
using Android.Widget;
using Newtonsoft.Json;
using OnlineFridge;
using test.DataAccess.Model;

namespace test
{
    [Activity(Label = "Register", Theme = "@style/CustomTheme")]
    public class Register : Activity
    {
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

            buttonRegister.Click += async (sender, e) =>
            {
                Toast.MakeText(this, "Ładowanie", ToastLength.Short).Show();

                var passwordToRegister = pass.Text.ToString();
                var emailToRegister = email.Text.ToString();

                // WYWOŁANIE METODY GetUser
                // POBRANIE UŻYTKOWNIKA O TAKIM SAMYM EMAILU JAK WPISANY
                var userToCheckIfExist = await GetUser(emailToRegister);

                if (userToCheckIfExist == null)         // SPRAWDZENIE CZY TAKI UŻYTKOWNIK ISTNIEJE
                {
                    // REJESTROWANIE
                    var u = new User();
                    u.email = emailToRegister;
                    u.password = passwordToRegister;
                    u.passwordHash = passwordToRegister;
                    u.username = emailToRegister;

                    await AddUser(u);       // DODANIE UŻYTKOWNIKA

                    Toast.MakeText(this, "Zarejestrowano!", ToastLength.Short).Show();
               }
               else
               {
                    Toast.MakeText(this, "Taki użytkownik już istnieje!", ToastLength.Short).Show();
               }
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

        // METODA DODAJACA UZYTKOWNIKA DO BAZY DANYCH
        public async Task AddUser(User user)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://192.168.0.102:61913/");

                var json = JsonConvert.SerializeObject(user);       // SERIALIZACJA OBIEKTU NA FORMAT JSON

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                await client.PostAsync("/api/User", content);
            }
        }
    }
}