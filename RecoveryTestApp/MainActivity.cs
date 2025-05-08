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
using System.Threading.Tasks;
using System.Threading;


namespace RecoveryTestApp
{
    [Activity(Label = "4oliow Reboot", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public partial class MainActivity : Activity
    {
        private CancellationTokenSource _cancellationTokenSource;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);


            CheckBox rootCheckBox = FindViewById<CheckBox>(Resource.Id.rootCheckBox);
            Button canceldialogButton = FindViewById<Button>(Resource.Id.canceldialogButton);
            Button fastbootButton = FindViewById<Button>(Resource.Id.fastbootButton);
            Button recoveryButton = FindViewById<Button>(Resource.Id.recoveryButton);
            TextView statusText = FindViewById<TextView>(Resource.Id.statusText);

            var roundCancel = new Android.Graphics.Drawables.GradientDrawable();
            roundCancel.SetCornerRadius(32f);
            roundCancel.SetColor(Android.Graphics.Color.Red);
            canceldialogButton.Background = roundCancel;
            canceldialogButton.SetTextColor(Android.Graphics.Color.White);

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


            canceldialogButton.Visibility = Android.Views.ViewStates.Gone;





            if (IsRooted())
            {
                rootCheckBox.Checked = true;
                statusText.Text = "Device is rooted.";
            }
            else
            {
                rootCheckBox.Checked = false;
                statusText.Text = "Device is not rooted, reboot buttons are disabled.";
            }


            recoveryButton.Click += (sender, e) =>
                    {
                        statusText.Text = "Initiating recovery reboot...";
                        SetupCancelTrigger();
                        StartCountdownRecovery();
                    };


            fastbootButton.Click += (sender, e) =>
                {
                    statusText.Text = "Initiating fastboot reboot...";
                    SetupCancelTrigger();
                    StartCountdownFastboot();
                };
        }








        
    }
}

