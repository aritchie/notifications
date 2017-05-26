using System;
using System.Diagnostics;
using Acr.Notifications;
using Xamarin.Forms;


namespace Samples
{

    public class App : Application
    {
        public static bool IsInBackgrounded { get; private set; }


        public App()
        {
            Notification.DefaultTitle = "Test Title";

            this.MainPage = new NavigationPage(new ContentPage
            {
                Title = "Notifications",
                Content = new StackLayout
                {
                    VerticalOptions = LayoutOptions.Center,
                    Children = {
                        new Button
                        {
                            Text = "Set Badge",
                            Command = new Command(() => CrossNotifications.Current.Badge = new Random().Next(100))
                        },
                        new Button
                        {
                            Text = "Clear Badge",
                            Command = new Command(() => CrossNotifications.Current.Badge = 0)
                        },
                        new Button
                        {
                            Text = "Press This & Exit App within 10 seconds",
                            Command = new Command(() =>
                                CrossNotifications.Current.Send(new Notification()
                                    .SetMessage("Hello from the ACR Sample Notification App")
                                    .SetVibrate(true)
                                    .SetSchedule(TimeSpan.FromSeconds(10))
                                )
                            )
                        },
                        new Button
                        {
                            Text = "Multiple Timed Messages (10 messages x 5 seconds apart)",
                            Command = new Command(() =>
                            {
                                CrossNotifications.Current.Send("Samples", "Starting Sample Schedule Notifications");
                                for (var i = 1; i < 11; i++)
                                {
                                    var seconds = i * 5;
                                    var id = CrossNotifications.Current.Send(new Notification()
                                        .SetMessage($"Message {i}")
                                        .SetSchedule(TimeSpan.FromSeconds(seconds))
                                    );
                                    Debug.WriteLine($"Notification ID: {id}");
                                }
                            })
                        },
                        new Button
                        {
                            Text = "Cancel All Notifications",
                            Command = new Command(CrossNotifications.Current.CancelAll)
                        },
                        new Button
                        {
                            Text = "Vibrate",
                            Command = new Command(() => CrossNotifications.Current.Vibrate())
                        }
                    }
                }
            });
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
