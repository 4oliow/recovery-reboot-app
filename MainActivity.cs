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
using AndroidX.Annotations;
using Javax.Security.Auth;

namespace RecoveryTestApp
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)] 
    public class MainActivity : Activity
    {

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);


            CheckBox rootCheckBox = FindViewById<CheckBox>(Resource.Id.rootCheckBox);
            Button fastbootButton = FindViewById<Button>(Resource.Id.fastbootButton);
            Button recoveryButton = FindViewById<Button>(Resource.Id.recoveryButton);
            TextView statusText = FindViewById<TextView>(Resource.Id.statusText);

            var roundRecovery = new Android.Graphics.Drawables.GradientDrawable();
            roundRecovery.SetCornerRadius(32f);
            roundRecovery.SetColor(Android.Graphics.Color.DarkRed);
            recoveryButton.Background = roundRecovery;
            recoveryButton.SetTextColor(Android.Graphics.Color.White);

            var roundFastboot = new Android.Graphics.Drawables.GradientDrawable();
            roundFastboot.SetCornerRadius(32f);
            roundFastboot.SetColor(Android.Graphics.Color.DarkBlue);
            fastbootButton.Background = roundFastboot;
            fastbootButton.SetTextColor(Android.Graphics.Color.White);



            
            
            if (IsRooted()) 
            {
                rootCheckBox.Checked = true;
                statusText.Text = "Device is rooted.";
            }
            else
            {
                rootCheckBox.Checked = false;
                statusText.Text = "Device is not rooted.";
            }
            

            recoveryButton.Click += (sender, e) =>
                    {
                        statusText.Text = "Initiating recovery reboot...";
                        ExecuteRebootToRecovery();
                    };


            fastbootButton.Click += (sender, e) =>
                {
                    statusText.Text = "Initiating fastboot reboot...";
                    ExecuteRebootToFastboot();
                };
        }
            
        private bool IsRooted()
        {
            try
            {
                Java.Lang.Runtime.GetRuntime().Exec("su");
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }       
        private void ExecuteRebootToRecovery()
        {
            TextView statusText = FindViewById<TextView>(Resource.Id.statusText);
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
                    statusText.Text = "testing only";
                });
            }
        }
        private void ExecuteRebootToFastboot()
        {
            TextView statusText = FindViewById<TextView>(Resource.Id.statusText);
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
                    statusText.Text = "testing only";
                });
            }
        }


    }  
}
	

