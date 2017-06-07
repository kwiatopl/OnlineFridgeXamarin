using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using OnlineFridge;
using test.DataAccess;
using test.DataAccess.Model;

namespace test
{
    [Activity(Label = "Tryb Offline")]
    public class OfflineMode : Activity
    {
       
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.OfflineMode);

            Button btnZawartosc = FindViewById<Button>(Resource.Id.button1);
            Button btnDodawanie = FindViewById<Button>(Resource.Id.button2);
        
            btnZawartosc.Click += (x, z) =>
            {
                var activity = new Intent(this, typeof(FridgeContent));
                StartActivity(activity);
            };
            
            btnDodawanie.Click += (x, z) =>
            {
                var activity = new Intent(this, typeof(AddProduct));
                StartActivity(activity);
            };
        }
    }
}