using System;
using Acr.Notifications;
using Xamarin.Forms;


namespace Samples {

    public class App : Application {

        public App() {
            this.MainPage = new ContentPage {
                Content = new StackLayout {
                    VerticalOptions = LayoutOptions.Center,
                    Children = {
                        new Button {
                            Text = "Press This & Exit App within 10 seconds",
                            Command = new Command(() => {
                                Notifications.Instance.Badge = 4;
                                Notifications.Instance.Send(
                                    "Test Message",
                                    "Hello from the ACR Sample Notification App",
                                    null,
                                    TimeSpan.FromSeconds(10)
                                );
                            })
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
