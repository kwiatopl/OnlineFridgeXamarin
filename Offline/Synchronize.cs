using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using OnlineFridge.DataAccess;
using OnlineFridge.DataAccess.Model;

namespace OnlineFridge.Offline
{
    [Activity(Label = "Synchronize", Theme = "@style/CustomTheme", NoHistory = true)]
    public class Synchronize : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Synchronize);

            var txtLogin = FindViewById<EditText>(Resource.Id.username);
            var txtPassword = FindViewById<EditText>(Resource.Id.password);

            var error = FindViewById<TextView>(Resource.Id.error);

            var btnPobierz = FindViewById<Button>(Resource.Id.buttonLogin);

            btnPobierz.Click += async (x, z) =>
            {
                var user = await GetUser(txtLogin.Text.ToString());

                if (!String.IsNullOrWhiteSpace(txtLogin.Text.ToString()) ||
                    !String.IsNullOrWhiteSpace(txtPassword.Text.ToString()))
                {
                    if (user.email != null)
                    {
                        if (IsValidLogin(txtLogin.Text.ToString()))
                        {
                            
                            if (txtLogin.Text.ToString() == user.email && HashAndSalt.Verify(user.salt, user.passwordHash, txtPassword.Text.ToString()))
                            {
                                SyncConfirmation(user.userId);
                            }
                            else
                            {
                                error.Text = "Błędne hasło lub email!";
                            }
                        }
                        else
                        {
                            error.Text = "Nieprawidłowy format email!";
                        }


                    }
                    else
                    {
                        error.Text = "Błędne hasło lub email!";
                    }
                }
                else
                {
                    Toast.MakeText(this, "Wypełnij pola!", ToastLength.Short).Show();
                }
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


        public async Task<User> GetUser(string email)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://192.168.0.101:61913/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var result = await client.GetAsync(String.Format("/api/User?email={0}", email));

                return JsonConvert.DeserializeObject<User>(await result.Content.ReadAsStringAsync());       // DESERIALIZACJA OBIEKTU Z FORMATU JSON NA OBIEKT KLASY USER
            }
        }

        public async Task<List<Product>> GetProduct(long userId)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://192.168.0.101:61913/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var result = await client.GetAsync(String.Format("/api/Product?userId={0}", userId));

                return JsonConvert.DeserializeObject<List<Product>>(await result.Content.ReadAsStringAsync());
            }
        }

        void Sync(List<Product> listToSync)
        {
            using(var db = new FridgeDb())
            {
                    var _productsList = db.Table<Product>().ToList();
                    foreach (Product prod in _productsList)
                    {
                        db.Delete(prod);
                    }

                    foreach (Product prod in listToSync)
                    {
                        db.Insert(prod);
                    }
            }
        }

        void SyncConfirmation(long userId)
        {
            AlertDialog.Builder alert = new AlertDialog.Builder(this);

            alert.SetTitle("Potwierdź import");
            alert.SetMessage("Czy na pewno chcesz zaimportować produkty?");

            alert.SetPositiveButton("Importuj", async (senderAlert, args) =>
            {
                var productsList = new List<Product>();
                productsList = await GetProduct(userId);
                Sync(productsList);
                Toast.MakeText(this, "Pomyślnie zsynchronizowano!", ToastLength.Short).Show();
            });

            alert.SetNegativeButton("Anuluj", (senderAlert, args) => { });

            Dialog dialog = alert.Create();
            dialog.Show();
        }

        protected override void OnStop()
        {
            base.OnStop();
            Finish();
        }

    }
}