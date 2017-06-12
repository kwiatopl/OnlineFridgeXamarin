using System;
using System.Globalization;
using System.Threading;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Newtonsoft.Json;
using OnlineFridge.DataAccess;
using OnlineFridge.DataAccess.Model;
using OnlineFridge.Online;
using OnlineFridge.SelectDate;

namespace OnlineFridge.Offline
{
    [Activity(Label = "Edycja produktu",Theme="@style/CustomTheme2", NoHistory = true)]
    public class UpdateProduct : Activity
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

            SetContentView(Resource.Layout.UpdateProduct);
            var MyJsonString = Intent.GetStringExtra("ProductToEdit");
            if (!string.IsNullOrWhiteSpace(MyJsonString))
            {
                var productEdit = JsonConvert.DeserializeObject<Product>(MyJsonString);
                Button btnDodaj = FindViewById<Button>(Resource.Id.button1);
                //EditText txtNazwa = FindViewById<EditText>(Resource.Id.editText1);
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
                    try
                    {

                        if ( //!string.IsNullOrWhiteSpace(txtNazwa.Text.ToString()) &&
                            int.Parse(txtIlosc.Text.ToString()) != 0
                        )
                        {
                            //productEdit.name = txtNazwa.Text.ToString().ToUpper();
                            productEdit.count = int.Parse(txtIlosc.Text.ToString());
                            productEdit.unit = type;
                            productEdit.expDate = _dateDisplay.Text.ToString().ToUpper();

                            using (var db = new FridgeDb())
                            {
                                db.Update(productEdit);
                            }

                            Toast.MakeText(this, "Edytowano", ToastLength.Short).Show();
                            var activity = new Intent(this, typeof(FridgeContent));
                            StartActivity(activity);
                        }
                        else
                        {
                            Toast.MakeText(this, "Wypełnij pola", ToastLength.Short).Show();
                        }
                    }
                    catch (OverflowException ex)
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