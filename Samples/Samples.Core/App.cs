using System;
using System.Diagnostics;
using Plugin.Notifications;
using Xamarin.Forms;


namespace Samples
{

    public class App : Application
    {
        public static bool IsInBackgrounded { get; private set; }


        public App()
        {

        }


        protected override void OnStart()
        {
            base.OnStart();
            CrossNotifications.Current.Activated += async (sender, notification) =>
            {
                Debug.WriteLine($"Notification Activated - {notification.Id} - {notification.Title}");
                await this.MainPage.DisplayAlert("Received Notification", notification.Message, "OK");
            };
        }


        protected override void OnResume()
        {
            base.OnResume();
            App.IsInBackgrounded = false;
        }


        protected override void OnSleep()
        {
            base.OnSleep();
            App.IsInBackgrounded = true;
        }
    }
}
