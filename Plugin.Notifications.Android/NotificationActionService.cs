using System;
using Android.App;
using Android.Content;
using Android.OS;


namespace Plugin.Notifications
{
    [Service]
    public class NotificationActionService : Service
    {
        public override IBinder OnBind(Intent intent)
        {
            throw new NotImplementedException();
        }
    }
}