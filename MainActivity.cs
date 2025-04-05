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
using Android.Graphics.Drawables;

namespace RecoveryTestApp
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)] 
    public class MainActivity : Activity
    {
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            Button fastbootButton = FindViewById<Button>(Resource.Id.fastbootButton);
            Button recoveryButton = FindViewById<Button>(Resource.Id.recoveryButton);
            TextView statusText = FindViewById<TextView>(Resource.Id.statusText);


            var roundRecovery = new Android.Graphics.Drawables.GradientDrawable();
            roundRecovery.SetCornerRadius(32f);
            roundRecovery.SetColor(Android.Graphics.Color.DarkRed);
            recoveryButton.Background = roundRecovery;

            var roundFastboot = new Android.Graphics.Drawables.GradientDrawable();
            roundFastboot.SetCornerRadius(32f);
            roundFastboot.SetColor(Android.Graphics.Color.DarkBlue);
            fastbootButton.Background = roundFastboot;



            recoveryButton.Click += (sender, e) =>
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
        private void ExecuteRebootToFastboot()
        {
            try
             {
                var proc = Java.Lang.Runtime.GetRuntime().Exec("su -c reboot fastboot");
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
	

