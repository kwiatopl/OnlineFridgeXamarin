using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using OnlineFridge;

namespace test
{
    [Activity(Label = "OnlineMode",Theme = "@style/CustomTheme")]
    public class OnlineMode : Activity
    {

        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.OnlineMode);

            TextView badLoginOrPass = FindViewById<TextView>(Resource.Id.badPass);

            EditText username = FindViewById<EditText>(Resource.Id.username);
            EditText pass = FindViewById<EditText>(Resource.Id.password);

            Button btnLogin = FindViewById<Button>(Resource.Id.buttonLogin);
            Button btnRegister = FindViewById<Button>(Resource.Id.buttonRegister);


            btnLogin.Click += (sender, e) =>
            {
                
                //kod
                var _activity = new Intent(this, typeof(AfterLogin));
                StartActivity(_activity);
            };
            btnRegister.Click += (sender, e) =>
            {
                var _activity = new Intent(this, typeof(Register));
                StartActivity(_activity);
            };

        }

       
        public static bool checkLogin(string _login, string _pass)
        {
            //kod
            return true;
        }
    }
}