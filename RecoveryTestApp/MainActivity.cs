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
    public class MainActivity : Activity
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
        private void SetupCancelTrigger()
        {
            Button canceldialogButton = FindViewById<Button>(Resource.Id.canceldialogButton);
            canceldialogButton.Click += (s, e) =>
            {
                _cancellationTokenSource?.Cancel();
                Toast.MakeText(this, "Reboot cancelled", ToastLength.Short).Show();
            };
        }

        private async void StartCountdownFastboot()
        {
            Button canceldialogButton = FindViewById<Button>(Resource.Id.canceldialogButton);
            Button recoveryButton = FindViewById<Button>(Resource.Id.recoveryButton);
            Button fastbootButton = FindViewById<Button>(Resource.Id.fastbootButton);
            TextView statusText = FindViewById<TextView>(Resource.Id.statusText);


            recoveryButton.Enabled = false;
            fastbootButton.Enabled = false;
            canceldialogButton.Visibility = Android.Views.ViewStates.Visible;

            _cancellationTokenSource = new CancellationTokenSource();

            try
            {
                for (int i = 5; i > 0; i--)
                {
                    if (_cancellationTokenSource.IsCancellationRequested)
                        break;

                    RunOnUiThread(() => canceldialogButton.Text = $"Rebooting in {i}s... (Tap to cancel)");
                    await Task.Delay(1000);
                }

                if (!_cancellationTokenSource.IsCancellationRequested)
                {
                    ExecuteRebootToFastboot();
                }

            }
            catch (Exception ex)
            {
                RunOnUiThread(() =>
                {
                    Toast.MakeText(this, $"Error: {ex.Message}", ToastLength.Long).Show();
                });
            }
            finally
            {
                canceldialogButton.Visibility = Android.Views.ViewStates.Gone;
                recoveryButton.Enabled = true;
                fastbootButton.Enabled = true;
                statusText.Text = "Reboot cancelled";
            }
        }
        private async void StartCountdownRecovery()
        {
            Button fastbootButton = FindViewById<Button>(Resource.Id.fastbootButton);
            Button recoveryButton = FindViewById<Button>(Resource.Id.recoveryButton);
            TextView canceldialogButton = FindViewById<Button>(Resource.Id.canceldialogButton);
            TextView statusText = FindViewById<TextView>(Resource.Id.statusText);

            fastbootButton.Enabled = false;
            recoveryButton.Enabled = false;
            canceldialogButton.Visibility = Android.Views.ViewStates.Visible;

            _cancellationTokenSource = new CancellationTokenSource();

            try
            {
                for (int i = 5; i > 0; i--)
                {
                    if (_cancellationTokenSource.IsCancellationRequested)
                        break;

                    RunOnUiThread(() => canceldialogButton.Text = $"Rebooting in {i}s... (Tap to cancel)");
                    await Task.Delay(1000);
                }

                if (!_cancellationTokenSource.IsCancellationRequested)
                {
                    ExecuteRebootToRecovery();
                }

            }
            catch (Exception ex)
            {
                RunOnUiThread(() =>
                {
                    Toast.MakeText(this, $"Error: {ex.Message}", ToastLength.Long).Show();
                });
            }
            finally
            {
                fastbootButton.Enabled = true;
                recoveryButton.Enabled = true;
                canceldialogButton.Visibility = Android.Views.ViewStates.Gone;
                statusText.Text = "Reboot cancelled";
            }






        }
    }
}

