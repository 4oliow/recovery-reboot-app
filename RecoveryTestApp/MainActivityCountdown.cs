using Android.Widget;
using System;
using System.Threading.Tasks;
using System.Threading;

namespace RecoveryTestApp
{
    public partial class MainActivity
    {
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