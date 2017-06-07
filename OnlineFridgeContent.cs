using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Util.Jar;
using Newtonsoft.Json;
using test;
using test.DataAccess.Model;





namespace OnlineFridge
{
    [Activity(Label = "OnlineFridgeContent")]
    public class OnlineFridgeContent : Activity
    {
        private ListView _listView;
        private List<ProductOnline> _productsList;

        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.OnlineFridgeContent);

            _listView = FindViewById<ListView>(Resource.Id.productListView);
            Button btn = FindViewById<Button>(Resource.Id.button1);
            Button btn2 = FindViewById<Button>(Resource.Id.button2);
            Button btn3 = FindViewById<Button>(Resource.Id.button3);
            Button btn4 = FindViewById<Button>(Resource.Id.button4);
            TextView textView = FindViewById<TextView>(Resource.Id.textView1);
            

            btn.Click += async(sender, args) =>
            {
                //GET
                var product = await GetProduct(21);
                Toast.MakeText(this, product.name, ToastLength.Short).Show();
            };

            btn2.Click += async (sender, args) =>
            {
                //PUT
                var productToUpdate = new ProductOnline
                    {
                    productId = 2,
                    name = "robak",
                    count = 3,
                    unit = (Quantity)2,
                    expDate = "2017-08-08",
                    userId = 2,
                    inShoppingList = false};
                await UpdateProduct(productToUpdate);

            };

            btn3.Click += async (sender, args) =>
            {
                //DELETE
                await DeleteProduct(22);
            };

            btn4.Click += async(sender, args) =>
            {
                //POST
                var productToAdd = new ProductOnline();

                //productToAdd.productId = 30;
                productToAdd.name = "slimak";
                productToAdd.count = 4;
                productToAdd.unit = (Quantity) 3;
                productToAdd.expDate = "2017-10-10";
                productToAdd.userId = 2;
                productToAdd.inShoppingList = false;

                await AddProduct(productToAdd);
            };

            //ListViewAdapter adapter = new ListViewAdapter(this, _productsList);
            //_listView.Adapter = adapter;

        }
        
        public async Task<List<ProductOnline>> GetProduct()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://192.168.0.102:52080/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.GetAsync("/api/Product");
                List<ProductOnline> productsList = await response.Content.ReadAsAsync<List<ProductOnline>>();
                return productsList;
            }

        }

        public async Task<ProductOnline> GetProduct(long id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://192.168.0.102:52080/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var result = await client.GetAsync(String.Format("/api/Product/{0}", id));

                return JsonConvert.DeserializeObject<ProductOnline>(await result.Content.ReadAsStringAsync());
            }
        }

        public async Task DeleteProduct(long id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://192.168.0.102:52080/");

                await client.DeleteAsync(String.Format("/api/Product/{0}", id));
            }
        }

        
        public async Task AddProduct(ProductOnline product)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://192.168.0.102:52080/");

                var json = JsonConvert.SerializeObject(product);

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                await client.PostAsync("/api/Product", content);

               // await client.PostAsJsonAsync<ProductOnline>("/api/Product", product);
            }
        }
        
        public async Task UpdateProduct(ProductOnline product)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://192.168.0.102:52080/");

                var json = JsonConvert.SerializeObject(product);

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                await client.PostAsync("/api/Product/{" + product.productId +  "}", content);
            }
        }


    }
}
