using System;


namespace Samples
{
    public class MainViewModel : ViewModel
    {
    }
}
/*
             CrossNotifications.Current.Activated += async (sender, notification) =>
            {
                Debug.WriteLine($"Notification Activated - {notification.Id} - {notification.Title}");
                await this.MainPage.DisplayAlert("Received Notification", notification.Message, "OK");
            };


Children =          Notification.DefaultTitle = "Test Title";
            var btnPermission = new Button {Text = "Request Permission"};
            btnPermission.Command = new Command(async () =>
            {
                var result = await CrossNotifications.Current.RequestPermission();
                btnPermission.Text = result ? "Permission Granted" : "Permission Denied";
            });

                    {
                        btnPermission,
                        new Button
                        {
                            Text = "Set Badge",
                            Command = new Command(() => CrossNotifications.Current.SetBadge(new Random().Next(100)))
                        },
                        new Button
                        {
                            Text = "Clear Badge",
                            Command = new Command(() => CrossNotifications.Current.SetBadge(0))
                        },
                        new Button
                        {
                            Text = "Press This & Exit App within 10 seconds",
                            Command = new Command(() =>
                                CrossNotifications.Current.Send(new Notification
                                {
                                    Title = "HELLO!",
                                    Message = "Hello from the ACR Sample Notification App",
                                    Vibrate = true,
                                    ScheduledDate = DateTime.Now.AddSeconds(10)
                                })
                            )
                        },
                        new Button
                        {
                            Text = "Send 10 messages x 5 seconds",
                            Command = new Command(() =>
                            {
                                CrossNotifications.Current.Send(new Notification
                                {
                                    Title = "Samples",
                                    Message = "Starting Sample Schedule Notifications"
                                });
                                for (var i = 1; i < 11; i++)
                                {
                                    var seconds = i * 5;
                                    var id = CrossNotifications.Current.Send(new Notification
                                    {
                                        Message = $"Message {i}",
                                        ScheduledDate = DateTime.Now.AddSeconds(seconds)
                                    });
                                    Debug.WriteLine($"Notification ID: {id}");
                                }
                            })
                        },
                        new Button
                        {
                            Text = "(Date) Send 10 messages x 5 seconds",
                            Command = new Command(() =>
                            {
                                CrossNotifications.Current.Send(new Notification
                                {
                                    Title = "Samples",
                                    Message = "Starting Sample Schedule Notifications"
                                });
                                for (var i = 1; i < 11; i++)
                                {
                                    var seconds = i * 5;
                                    var id = CrossNotifications.Current.Send(new Notification
                                    {
                                        Message = $"Message {i}",
                                        ScheduledDate = DateTime.Now.AddSeconds(seconds)
                                    });
                                    Debug.WriteLine($"Notification ID: {id}");
                                }
                            })
                        },
                        new Button
                        {
                            Text = "Cancel All Notifications",
                            Command = new Command(() => CrossNotifications.Current.CancelAll())
                        },
                        new Button
                        {
                            Text = "Vibrate",
                            Command = new Command(() => CrossNotifications.Current.Vibrate())
                        }
                    }
 */