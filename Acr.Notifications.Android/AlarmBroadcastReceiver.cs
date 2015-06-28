using System;
using Android.App;
using Android.Content;


namespace Acr.Notifications {

    [BroadcastReceiver]
    [IntentFilter(new [] { Intent.ActionBootCompleted }, Priority = Int32.MaxValue)]
    public class AlarmBroadcastReceiver : BroadcastReceiver {

        public override void OnReceive(Context context, Intent intent) {
            var notification = intent.FromIntent();
            Notifications.Instance.Send(notification); // resend without schedule so it goes through normal mechanism
        }
    }
}