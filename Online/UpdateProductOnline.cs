using System;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Newtonsoft.Json;
using OnlineFridge.DataAccess.Model;
using OnlineFridge.SelectDate;

namespace OnlineFridge.Online
{
    [Activity(Label = "Edycja produktu",Theme="@style/CustomTheme2", NoHistory = true)]
    public class UpdateProductOnline : Activity
    {
        
        TextView _dateDisplay;
        Button _dateSelectButton;
        private Quantity type;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.UpdateProduct);

            
            var MyJsonString = Intent.GetStringExtra("ProductToEdit");

            Intent.RemoveExtra("ProductToEdit");

            if (!string.IsNullOrWhiteSpace(MyJsonString))
            {
                var productEdit = new ProductOnline();
                productEdit = JsonConvert.DeserializeObject<ProductOnline>(MyJsonString);

                Button btnDodaj = FindViewById<Button>(Resource.Id.button1);
                TextView txtActualCount = FindViewById<TextView>(Resource.Id.textView5);
                TextView txtActualName = FindViewById<TextView>(Resource.Id.txtProduct);
                EditText txtIlosc = FindViewById<EditText>(Resource.Id.editText2);
                RadioButton kilogramy = FindViewById<RadioButton>(Resource.Id.kilogramy);
                RadioButton dekagramy = FindViewById<RadioButton>(Resource.Id.dekagramy);
                RadioButton gramy = FindViewById<RadioButton>(Resource.Id.gramy);
                RadioButton litry = FindViewById<RadioButton>(Resource.Id.litry);
                RadioButton mililitry = FindViewById<RadioButton>(Resource.Id.mililitry);
                RadioButton sztuki = FindViewById<RadioButton>(Resource.Id.sztuki);

                string actualCount = productEdit.count + " " + productEdit.unit;

                string actualExpDate = productEdit.expDate;

                string actualName = productEdit.name;

                Quantity actualType = productEdit.unit;

                txtActualName.Text = actualName;

                txtActualCount.Text = actualCount;

                switch (actualType)
                {
                    case Quantity.kg:
                        kilogramy.Checked = true;
                        type = Quantity.kg;
                        break;
                    case Quantity.dek:
                        dekagramy.Checked = true;
                        type = Quantity.dek;
                        break;
                    case Quantity.g:
                        gramy.Checked = true;
                        type = Quantity.g;
                        break;
                    case Quantity.l:
                        litry.Checked = true;
                        type = Quantity.l;
                        break;
                    case Quantity.ml:
                        mililitry.Checked = true;
                        type = Quantity.ml;
                        break;
                    case Quantity.sztuk:
                        sztuki.Checked = true;
                        type = Quantity.sztuk;
                        break;
                }

                _dateDisplay = FindViewById<TextView>(Resource.Id.date_display);
                _dateSelectButton = FindViewById<Button>(Resource.Id.date_select_button);

                _dateSelectButton.Click += DateSelect_OnClick;

                _dateDisplay.Text = actualExpDate;

                kilogramy.Click += (x, z) =>
                {
                    type = Quantity.kg;
                };

                dekagramy.Click += (x, z) =>
                {
                    type = Quantity.dek;
                };

                gramy.Click += (x, z) =>
                {
                    type = Quantity.g;
                };

                litry.Click += (x, z) =>
                {
                    type = Quantity.l;
                };

                mililitry.Click += (x, z) =>
                {
                    type = Quantity.ml;
                };

                sztuki.Click += (x, z) =>
                {
                    type = Quantity.sztuk;
                };

                btnDodaj.Click += (x, z) =>
                {
                    if (int.Parse(txtIlosc.Text.ToString()) != 0)
                    {
                        productEdit.count = int.Parse(txtIlosc.Text.ToString());
                        productEdit.unit = type;
                        productEdit.expDate = _dateDisplay.Text.ToString().ToUpper();

                        Update(productEdit);
                        Toast.MakeText(this, "Edytowano!", ToastLength.Short).Show();

                        var activity = new Intent(this, typeof(OnlineFridgeContent));

                            activity.SetFlags(ActivityFlags.NewTask);
                            activity.SetFlags(ActivityFlags.ClearTask);

                            StartActivity(activity);
                        
                     
                    }
                    else
                    {
                        Toast.MakeText(this, "Wypełnij pola", ToastLength.Short).Show();
                    }
                };
            }
            else
            {
                Toast.MakeText(this, "Błąd! Brak obiektu!", ToastLength.Short).Show();
                var activity = new Intent(this,typeof(OnlineFridgeContent));
                StartActivity(activity);
            }
        }

        void DateSelect_OnClick(object sender, EventArgs eventArgs)
        {
            DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
            {
                _dateDisplay.Text = time.ToString("yyyy-MM-dd");
            });
            frag.Show(FragmentManager, DatePickerFragment.TAG);
        }

        private async void Update(ProductOnline productToPut)
        {
                await UpdateProduct(productToPut);   
        }

       public async Task UpdateProduct(ProductOnline product)
       {
           using (var client = new HttpClient())
           {
               
               client.BaseAddress = new Uri("http://192.168.1.17:61913/");

               var json = JsonConvert.SerializeObject(product);

               var content = new StringContent(json, Encoding.UTF8, "application/json");
               var response = await client.PutAsync("/api/Product", content);        
            }
       }
       

        protected override void OnStop()
        {
            base.OnStop();
            Finish();
        }
    }
}