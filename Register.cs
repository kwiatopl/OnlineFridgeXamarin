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
    [Activity(Label = "Register", Theme = "@style/CustomTheme")]
    public class Register : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Register);

            EditText email = FindViewById<EditText>(Resource.Id.email);
            EditText pass = FindViewById<EditText>(Resource.Id.pass);

            TextView error = FindViewById<TextView>(Resource.Id.error);

            Button buttonRegister = FindViewById<Button>(Resource.Id.buttonRegister);

            buttonRegister.Click += (sender, e) =>
            {

                /*
                if (CheckPassword(pass.ToString()))
                {
                    // HASLO PRAWIDLOWE
                    // DALSZY KOD
                    error.SetText("OK!",TextView.BufferType.Normal);
                }
                else
                {
                    //Hasło musi zawierać przynajmniej jedną: małą literę, dużą literę, cyfrę, znak specjalny!
                    error.SetText("Hasło musi zawierać przynajmniej jedną: małą literę, dużą literę, cyfrę, znak specjalny!",TextView.BufferType.Normal);
                }
                */
                Toast.MakeText(this, "Zarejestrowano!", ToastLength.Short).Show();
            };
        }
        
        // DO POPRAWY
        /* 
        public bool CheckPassword(string _password)
        {

            var hasNumber = new Regex(@"[0-9]+");
            //var hasLowerChar = new Regex(@"[a-z]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasMinimum8Chars = new Regex(@".{8,}");

            bool flag = false;

            
            string specialChar = @"\|!#$%&/()=?»«@£§€{}.-;'<>_,";
            foreach (var item in specialChar)
            {
                if (_password.Contains(item))
                {
                    flag = true;
                }
            }
            

            if(hasNumber.IsMatch(_password) && hasUpperChar.IsMatch(_password) && hasMinimum8Chars.IsMatch(_password))
            {
                return true;
            }
            return false;
        }
    */

   
    }
}