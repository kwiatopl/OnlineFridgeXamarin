using System;
using System.Net.Http;
using System.Text;
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
    
[Activity(Label = "Dodawanie Produktu",Theme = "@style/CustomTheme2", NoHistory = true)]
    public class AddProductOnline : Activity
    {    
        TextView _dateDisplay;
        Button _dateSelectButton;
        Quantity type;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.AddProduct);

            var actualUser = new User();

            var json = Intent.GetStringExtra("userDodawanie");
            if (json != null)
            {
                actualUser = JsonConvert.DeserializeObject<User>(json);
            }

            Intent.RemoveExtra("userDodawanie");

            Button btnDodaj = FindViewById<Button>(Resource.Id.button1);
            EditText txtNazwa = FindViewById<EditText>(Resource.Id.editText1);
            EditText txtIlosc = FindViewById<EditText>(Resource.Id.editText2);
            RadioButton kilogramy = FindViewById<RadioButton>(Resource.Id.kilogramy);
            RadioButton dekagramy = FindViewById<RadioButton>(Resource.Id.dekagramy);
            RadioButton gramy = FindViewById<RadioButton>(Resource.Id.gramy);
            RadioButton litry = FindViewById<RadioButton>(Resource.Id.litry);
            RadioButton mililitry = FindViewById<RadioButton>(Resource.Id.mililitry);
            RadioButton sztuki = FindViewById<RadioButton>(Resource.Id.sztuki);

            _dateDisplay = FindViewById<TextView>(Resource.Id.date_display);
            _dateSelectButton = FindViewById<Button>(Resource.Id.date_select_button);
            _dateSelectButton.Click += DateSelect_OnClick;

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
                try
                {
                    if (!string.IsNullOrWhiteSpace(txtNazwa.Text.ToString()) &&
                        int.Parse(txtIlosc.Text.ToString()) != 0)
                    {

                        ProductOnline produkt = new ProductOnline();

                        produkt.productId = 0;
                        produkt.name = txtNazwa.Text.ToString().ToUpper();
                        produkt.count = int.Parse(txtIlosc.Text.ToString());
                        produkt.unit = type;
                        produkt.expDate = _dateDisplay.Text.ToString().ToUpper();
                        produkt.userId = actualUser.userId;
                        produkt.inShoppingList = false;

                        PostProduct(produkt);

                        Toast.MakeText(this, "Dodano", ToastLength.Short).Show();
                        var activity = new Intent(this, typeof(AddProductOnline));
                        StartActivity(activity);
                    }
                    else
                    {
                        Toast.MakeText(this, "Wypełnij pola", ToastLength.Short).Show();
                    }
                }
                catch (System.OverflowException ex)
                {
                    AlertDialog.Builder messageBox = new AlertDialog.Builder(this);
                    messageBox.SetTitle("Dodawanie produktu");
                    messageBox.SetMessage(ex.Message);
                    messageBox.SetCancelable(false);
                    messageBox.SetNeutralButton("OK", (senderAlert, args) =>
                    {

                    });
                    Dialog dialog = messageBox.Create();
                    dialog.Show();
                }
            };

        }

        void DateSelect_OnClick(object sender, EventArgs eventArgs)
        {
            DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
            {
                _dateDisplay.Text = time.ToString("yyyy-MM-dd");
            });
            frag.Show(FragmentManager, DatePickerFragment.TAG);
        }

        protected override void OnStop()
        {
            base.OnStop();
            Finish();
        }
        
        private async void PostProduct(ProductOnline productToPost)
        {
            await AddProduct(productToPost);
        }
        
        public async Task AddProduct(ProductOnline product)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://192.168.0.103:61913/");

                var json = JsonConvert.SerializeObject(product);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync("/api/Product", content);
            }
        }
    }
   
}