using System;
using System.Collections;
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
using test.DataAccess;
using test.DataAccess.Model;





namespace OnlineFridge
{
    [Activity(Label = "Zawartość Lodówki", NoHistory = true)]
    public class OnlineFridgeContent : Activity
    {
        private ListView _listView;
        List<ProductOnline> _productsList = new List<ProductOnline>();

        protected override async void  OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.FridgeContent);

            _listView = FindViewById<ListView>(Resource.Id.productListView);

            var json = Intent.GetStringExtra("SerializedUser");
            var actualUser = JsonConvert.DeserializeObject<User>(json);

            _productsList = await GetProduct((long)actualUser.userId);

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

            alert.SetPositiveButton("Usuń", (senderAlert, args) =>
            {
                var itemToRemove = _productsList.Single(r => r.productId == product.productId);
                _productsList.Remove(itemToRemove);

                DeleteAsync(itemToRemove.productId);

                Toast.MakeText(this, "USUNIĘTO! ", ToastLength.Short).Show();

                var activity = new Intent(this, typeof(OnlineFridgeContent));
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

                var activity = new Intent(this, typeof(UpdateProduct));

                activity.PutExtra("ProductToEdit", SerializedObject);

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
                client.BaseAddress = new Uri("http://192.168.0.102:61913/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var result = await client.GetAsync(String.Format("/api/Product?userId={0}", userId));

                return JsonConvert.DeserializeObject<List<ProductOnline>>(await result.Content.ReadAsStringAsync());
            }
        }

        public async Task DeleteProduct(long id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://192.168.0.102:61913/");

                await client.DeleteAsync(String.Format("/api/Product/{0}", id));
            }
        }


    }

}
