using System;
using Acr.Notifications;
using Xamarin.Forms;


namespace Samples {

    public class App : Application {

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
    }
}
