using System;

using System.Globalization;
using System.Net.Http;
using Android.App;
using Android.Content;
using Android.OS;

using Android.Widget;
using OnlineFridge;
using test.DataAccess;
using test.DataAccess.Model;
using test.SelectDate;
using Thread = System.Threading.Thread;

namespace test
{ 
    
[Activity(Label = "Dodawanie Produktu",Theme="@style/CustomTheme2", NoHistory = true)]
    public class AddProduct : Activity
    { 

       
        TextView _dateDisplay;
        Button _dateSelectButton;
        Quantity type;
       

        protected override void OnResume()
        {
            base.OnResume();

            // Ustawienie kultury
            var userSelectedCulture = new CultureInfo("pl-PL");
            Thread.CurrentThread.CurrentCulture = userSelectedCulture;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.AddProduct);

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
                    if (!string.IsNullOrWhiteSpace(txtNazwa.Text.ToString()) && int.Parse(txtIlosc.Text.ToString()) != 0 )
                    {
                    Product produkt = new Product();

                    produkt.name = txtNazwa.Text.ToString().ToUpper();
                    produkt.count = int.Parse(txtIlosc.Text.ToString());
                    produkt.unit = type;
                    produkt.expDate = _dateDisplay.Text.ToString().ToUpper();

                    using (var db = new FridgeDb())
                    {
                        db.Insert(produkt);
                    }

                    // TEST 

                    Toast.MakeText(this, "Dodano", ToastLength.Short).Show();
                        var activity = new Intent(this, typeof(AddProduct));
                        StartActivity(activity);

                    }
                    else
                    {
                    Toast.MakeText(this, "Wypełnij pola", ToastLength.Short).Show();
                    }    

               
            };

        }

        

        void DateSelect_OnClick(object sender, EventArgs eventArgs)
        {
            DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
            {
                _dateDisplay.Text = time.ToShortDateString();
            });
            frag.Show(FragmentManager, DatePickerFragment.TAG);
        }

        protected override void OnStop()
        {
            base.OnStop();
            Finish();
        }
    }
   
}