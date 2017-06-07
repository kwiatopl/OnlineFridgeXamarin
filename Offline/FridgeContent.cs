
using System;
using System.Collections.Generic;

using System.Linq;
using System.Threading;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;

using Android.Widget;

using Newtonsoft.Json;
using OnlineFridge;
using test.DataAccess;
using test.DataAccess.Model;

namespace test
{
    [Activity(Label = "Zawartość Lodówki", NoHistory = true)] 
    public class FridgeContent : Activity
    {
        
        private ListView _listView;
        private List<Product> _productsList;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.FridgeContent);
            _listView = FindViewById<ListView>(Resource.Id.productListView);

            _productsList = new List<Product>();

            using (var db = new FridgeDb())
            {
                _productsList = db.Table<Product>().ToList();
            }

            ListViewAdapter adapter = new ListViewAdapter(this, _productsList);
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

        void DeleteConfirmation(Product product)
        {
            AlertDialog.Builder alert = new AlertDialog.Builder(this);

            alert.SetTitle("Potwierdź usunięcie");
            alert.SetMessage("Czy na pewno chcesz usunąć ten produkt z listy?");

            alert.SetPositiveButton("Usuń", (senderAlert, args) =>
            {
                var itemToRemove = _productsList.Single(r => r.productId == product.productId);
                _productsList.Remove(itemToRemove);
                using (var db = new FridgeDb())
                {
                        db.Delete(itemToRemove);
                }
                Toast.MakeText(this, "USUNIĘTO! ", ToastLength.Short).Show();
                var activity = new Intent(this,typeof(FridgeContent));
                StartActivity(activity);
            });

            alert.SetNegativeButton("Anuluj", (senderAlert, args) =>
            { });

            Dialog dialog = alert.Create();
            dialog.Show();
        }

        void EditConfirmation(Product product)
        {
            AlertDialog.Builder alert = new AlertDialog.Builder(this);

            alert.SetTitle("Potwierdź edycje");
            alert.SetMessage("Czy na pewno chcesz edytować ten produkt?");

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
    }
}