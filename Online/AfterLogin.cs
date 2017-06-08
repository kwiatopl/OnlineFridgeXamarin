using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using OnlineFridge;
using test.DataAccess.Model;

namespace test
{
    [Activity(Label = "Tryb Online")]
    public class AfterLogin : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.AfterLogin);

            Button btnZawartosc = FindViewById<Button>(Resource.Id.button1);
            Button btnDodawanie = FindViewById<Button>(Resource.Id.button2);
            Button btnKonto = FindViewById<Button>(Resource.Id.button3);
            TextView textView = FindViewById<TextView>(Resource.Id.textView1);

            btnZawartosc.Click += (x, z) =>
            {
                var activity = new Intent(this, typeof(OnlineFridgeContent));
                StartActivity(activity);
            };

           

            btnKonto.Click += (x, z) =>
            {
                var activity = new Intent(this, typeof(EditAccount));
                StartActivity(activity);
            };
        }
    }
}