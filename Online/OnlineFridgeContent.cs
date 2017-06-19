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
using OnlineFridge.DataAccess.Model;
using OnlineFridge.Offline;

namespace OnlineFridge.Online
{
    [Activity(Label = "Zawartość Lodówki",Theme="@style/CustomTheme2", NoHistory = true)]
    public class OnlineFridgeContent : Activity
    {
        private ListView _listView;
        List<ProductOnline> _productsList = new List<ProductOnline>();

        User actualUser = new User();
        /*
        protected override async void OnResume()
        {
            base.OnResume();

            var json = Intent.GetStringExtra("userZawartosc");
            var actualUser = JsonConvert.DeserializeObject<User>(json);

            _productsList = null;

            Intent.RemoveExtra("userZawartosc");

            Toast.MakeText(this, "Ładowanie...", ToastLength.Short).Show();

            _productsList = await GetProduct((long)actualUser.userId);

            var adapter = new ListViewAdapterOnline(this, _productsList);
            _listView.Adapter = adapter;

            _listView.ItemClick += ListView_ItemClick;
            _listView.ItemLongClick += ListView_ItemLongClick;
        }
        */

        protected override async void  OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.FridgeContent);

            _listView = FindViewById<ListView>(Resource.Id.productListView);

            var json = Intent.GetStringExtra("userZawartosc");
            actualUser = JsonConvert.DeserializeObject<User>(json);

            Toast.MakeText(this, "Ładowanie...", ToastLength.Short).Show();

            _productsList = await GetProduct((long)actualUser.userId);
            foreach (ProductOnline p in _productsList)
            {
                if (p.expDate == "0001-01-01")
                {
                    p.expDate = null;
                }
            }

            var adapter = new ListViewAdapterOnline(this, _productsList);
            _listView.Adapter = adapter;

            _listView.ItemClick += ListView_ItemClick;
            _listView.ItemLongClick += ListView_ItemLongClick;
        }

        void ListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            EditConfirmation(_productsList[e.Position]);
        }

        void ListView_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            DeleteConfirmation(_productsList[e.Position]);
        }

        void DeleteConfirmation(ProductOnline product)
        {
            AlertDialog.Builder alert = new AlertDialog.Builder(this);

            alert.SetTitle("Potwierdź usunięcie");
            alert.SetMessage("Czy na pewno chcesz usunąć produkt: " + product.name + " z listy?");

            alert.SetPositiveButton("Usuń", async (senderAlert, args) =>
            {
                var itemToRemove = _productsList.Single(r => r.productId == product.productId);
                _productsList.Remove(itemToRemove);

                await DeleteProduct(product.productId);
                Toast.MakeText(this, "Usunięto!", ToastLength.Short).Show();

                var activity = new Intent(this, typeof(OnlineFridgeContent));
                var jsonUser = JsonConvert.SerializeObject(actualUser);
                activity.PutExtra("userZawartosc", jsonUser);
                StartActivity(activity);
            });

            alert.SetNegativeButton("Anuluj", (senderAlert, args) =>
                { });

            Dialog dialog = alert.Create();
            dialog.Show();
        }

        
        private async void DeleteAsync(long id)
        {
            await DeleteProduct(id);
        }
        

        void EditConfirmation(ProductOnline product)
        {
            AlertDialog.Builder alert = new AlertDialog.Builder(this);

            alert.SetTitle("Potwierdź edycje");
            alert.SetMessage("Czy na pewno chcesz edytować produkt " + product.name + " ?");

            alert.SetPositiveButton("Edytuj", (senderAlert, args) =>
            {
                var itemToUpdate = _productsList.Single(r => r.productId == product.productId);

                var SerializedObject = JsonConvert.SerializeObject(itemToUpdate);
                var SerializedUser = JsonConvert.SerializeObject(actualUser);

                var activity = new Intent(this, typeof(UpdateProductOnline));

                activity.PutExtra("ProductToEdit", SerializedObject);
                activity.PutExtra("User", SerializedUser);
                StartActivity(activity);
                
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

        public async Task<List<ProductOnline>> GetProduct(long userId)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://192.168.0.101:61913/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var result = await client.GetAsync(String.Format("/api/Product?userId={0}", userId));

                if (result.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<List<ProductOnline>>(await result.Content.ReadAsStringAsync());
                }
                else
                {
                    Toast.MakeText(this, "Wystąpił błąd", ToastLength.Short).Show();
                    return null;
                }
            }
        }

        public async Task DeleteProduct(long id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://192.168.0.101:61913/");

                var response = await client.DeleteAsync(String.Format("/api/Product/{0}", id));
                
             }
        }


    }

}
