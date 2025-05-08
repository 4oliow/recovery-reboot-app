using Android.Widget;
using System;

namespace RecoveryTestApp
{
    public partial class MainActivity
    {
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