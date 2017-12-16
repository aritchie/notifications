using System;
using System.Threading.Tasks;
using AudioToolbox;
using UIKit;


namespace Plugin.Notifications
{
    public abstract class AbstractAppleNotificationsImpl : AbstractNotificationsImpl
    {
        public override Task<int> GetBadge()
        {
            var tcs = new TaskCompletionSource<int>();
            UIApplication.SharedApplication.InvokeOnMainThread(() =>
            {
                var value = (int)UIApplication.SharedApplication.ApplicationIconBadgeNumber;
                tcs.TrySetResult(value);
            });
            return tcs.Task;
        }


        public override Task SetBadge(int value) => this.Invoke(() =>
            UIApplication.SharedApplication.ApplicationIconBadgeNumber = value
        );


        public override void Vibrate(int ms) => SystemSound.Vibrate.PlaySystemSound();


        protected Task Invoke(Action action)
        {
            var tcs = new TaskCompletionSource<object>();
            UIApplication.SharedApplication.InvokeOnMainThread(() =>
            {
                action();
                tcs.TrySetResult(null);
            });
            return tcs.Task;
        }
    }
}
