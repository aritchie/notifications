using System;
using Acr.Notifications;
using Xamarin.Forms;


namespace Samples {

    public class App : Application {
        public static bool IsInBackgrounded { get; private set; }


        public App() {
            Notification.DefaultTitle = "Test Title";

            this.MainPage = new ContentPage {
                Content = new StackLayout {
                    VerticalOptions = LayoutOptions.Center,
                    Children = {
                        new Button {
                            Text = "Set Badge",
                            Command = new Command(() => Notifications.Instance.Badge = 1)
                        },
                        new Button {
                            Text = "Press This & Exit App within 10 seconds",
                            Command = new Command(() =>
                                Notifications.Instance.Send(new Notification()
                                    .SetMessage("Hello from the ACR Sample Notification App")
                                    .SetSchedule(TimeSpan.FromSeconds(10))
                                    .AddAction("Test", Guid.NewGuid().ToString(), false, false)
                                    .AddAction("Destroy", Guid.NewGuid().ToString(), true)
                                )
                            )
                        },
                        new Button {
                            Text = "Cancel All Notifications",
                            Command = new Command(Notifications.Instance.CancelAll)
                        }
                    }
                }
            };
        }


        protected override void OnResume() {
            base.OnResume();
            App.IsInBackgrounded = false;
        }


        protected override void OnSleep() {
            base.OnSleep();
            App.IsInBackgrounded = true;
        }
    }
}
