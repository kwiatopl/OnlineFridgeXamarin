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

namespace OnlineFridge
{
    [Activity(Label = "ExitActivity")]
    public class ExitActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            FinishAndRemoveTask();
        }

        public static void exitApplication(Context context)
        {
            Intent intent = new Intent(context, typeof(ExitActivity));

            intent.SetFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask | ActivityFlags.NoAnimation| ActivityFlags.ExcludeFromRecents);

            context.StartActivity(intent);
        }
}
}