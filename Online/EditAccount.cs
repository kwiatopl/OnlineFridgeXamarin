using System;
using System.Net.Http;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Newtonsoft.Json;
using OnlineFridge.DataAccess.Model;

namespace OnlineFridge.Online
{
    [Activity(Label = "Edycja konta",Theme="@style/CustomTheme")]
    public class EditAccount : Activity
    {
        private bool flag;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.EditAccount);

            flag = false;
            var json = Intent.GetStringExtra("userKonto");
            var actualUser = JsonConvert.DeserializeObject<User>(json);

            Toast.MakeText(this, actualUser.email, ToastLength.Short).Show();

            Intent.RemoveExtra("userKonto");

            var btnChangePass = FindViewById<Button>(Resource.Id.passChange);
            var btnDelUser = FindViewById<Button>(Resource.Id.accountDel);

            btnChangePass.Click += (x, z) =>
            {
                var activity = new Intent(this, typeof(ChangePassword));

                var userToChangePass = JsonConvert.SerializeObject(actualUser);

                activity.PutExtra("userChangePass", userToChangePass);

                StartActivity(activity);
            };

            btnDelUser.Click += (x, z) =>
            {
                DeleteConfirmation(actualUser.userId);
            };

        }

        void DeleteConfirmation(int userIdToDelete)
        {
            AlertDialog.Builder alert = new AlertDialog.Builder(this);

            alert.SetTitle("Potwierdź usunięcie");
            alert.SetMessage("Czy na pewno chcesz usunąć konto?");

            alert.SetPositiveButton("Usuń", (senderAlert, args) =>
            {
                DeleteAsync(userIdToDelete);

                Toast.MakeText(this, "Usunięto użytkownika!", ToastLength.Short).Show();
                var activity = new Intent(this, typeof(MainActivity));

                    StartActivity(activity);
            });

            alert.SetNegativeButton("Anuluj", (senderAlert, args) =>
                { });

            Dialog dialog = alert.Create();
            dialog.Show();
        }

        private async void DeleteAsync(long id)
        {
            await DeleteUser(id);
        }

        public async Task DeleteUser(long id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://192.168.1.17:61913/");

                var response = await client.DeleteAsync(String.Format("/api/User/{0}", id));
            }
        }
    }
}