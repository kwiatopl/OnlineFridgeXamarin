using System;
using System.Collections.Generic;
using System.Linq;
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
    [Activity(Label = "Twoja lodówka", Theme = "@style/CustomTheme")]
    public class AfterLogin : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
            //POBRANIE DANYCH AKTUALNEGO UŻYTKOWNIKA
            var json = Intent.GetStringExtra("ActualUser");
            var actualUser = JsonConvert.DeserializeObject<User>(json);

            SetContentView(Resource.Layout.AfterLogin);

            Button btnZawartosc = FindViewById<Button>(Resource.Id.fridgeContent);
            Button btnDodawanie = FindViewById<Button>(Resource.Id.addProduct);
            Button btnKonto = FindViewById<Button>(Resource.Id.editAccount);
            Button btnWyloguj = FindViewById<Button>(Resource.Id.logout);

            btnZawartosc.Click += (x, z) =>
            {
                //var productsList = new List<ProductOnline>();
                //productsList = await GetProduct();

                var activity = new Intent(this, typeof(OnlineFridgeContent));

                //var productsListSerialized = JsonConvert.SerializeObject(productsList);

                activity.PutExtra("SerializedUser", json);
               // activity.PutExtra("ProductsList", productsListSerialized);
                StartActivity(activity);
            };

            btnDodawanie.Click += (x, z) =>
            {
                var activity = new Intent(this, typeof(AddProductOnline));

                activity.PutExtra("SerializedUser", json);

                StartActivity(activity);
            };

            btnKonto.Click += (x, z) =>
            {
                var activity = new Intent(this, typeof(EditAccount));

                activity.PutExtra("SerializedUser", json);

                StartActivity(activity);
            };

            btnWyloguj.Click += (x, z) =>
            {
                var activity = new Intent(this, typeof(OnlineMode));
                StartActivity(activity);
            };
        }

        
        public async Task<List<ProductOnline>> GetProduct()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://192.168.0.102:52080/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.GetAsync("/api/Product");
                return JsonConvert.DeserializeObject<IEnumerable<ProductOnline>>(await response.Content.ReadAsStringAsync()).ToList();
            }
        }
        
    }
}