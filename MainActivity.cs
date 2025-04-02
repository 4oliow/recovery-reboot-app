using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.Widget;
using AndroidX.AppCompat.App;
using Google.Android.Material.FloatingActionButton;
using Google.Android.Material.Snackbar;
using Android.Widget;

namespace RecoveryTestApp
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            Button rebootButton = FindViewById<Button>(Resource.Id.rebootButton);
            TextView statusText = FindViewById<TextView>(Resource.Id.statusText);

            rebootButton.Click += (sender, e) =>
            {
                statusText.Text = "Initiating recovery reboot...";
                ExecuteRebootToRecovery();
            };
        }

        private void ExecuteRebootToRecovery()
        {
            try
            {
                var proc = Java.Lang.Runtime.GetRuntime().Exec("su -c reboot recovery");
                proc.WaitFor();
            }
            catch (Exception ex)
            {
                RunOnUiThread(() =>
                {
                    Toast.MakeText(this, $"Failed: {ex.Message}", ToastLength.Long).Show();
                });
            }
        }
    }
}
	

