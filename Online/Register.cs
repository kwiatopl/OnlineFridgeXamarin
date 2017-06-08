using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
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

            EditText email = FindViewById<EditText>(Resource.Id.email);
            EditText pass = FindViewById<EditText>(Resource.Id.pass);

            TextView error = FindViewById<TextView>(Resource.Id.error);

            Button buttonRegister = FindViewById<Button>(Resource.Id.buttonRegister);

            buttonRegister.Click += async (sender, e) =>
            {
                var passwordToRegister = pass.Text.ToString();
                var emailToRegister = email.Text.ToString();
                var userToCheckIfExist = await GetUser(emailToRegister);
                if (userToCheckIfExist == null)
                {
                    // REJESTROWANIE
                    var u = new User();
                    u.email = emailToRegister;
                    u.password = passwordToRegister;
                    u.passwordHash = passwordToRegister;
                    u.username = emailToRegister;

                    await AddUser(u);

                    Toast.MakeText(this, "Zarejestrowano!", ToastLength.Short).Show();
               }
               else
               {
                    Toast.MakeText(this, "Taki użytkownik już istnieje!", ToastLength.Short).Show();
               }
            };
        }

        public async Task<User> GetUser(string email)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://192.168.0.102:61913/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var result = await client.GetAsync(String.Format("/api/User?email={0}", email));

                return JsonConvert.DeserializeObject<User>(await result.Content.ReadAsStringAsync());
            }
        }

        public async Task AddUser(User user)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://192.168.0.102:61913/");

                var json = JsonConvert.SerializeObject(user);

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                await client.PostAsync("/api/User", content);
            }
        }
    }
}