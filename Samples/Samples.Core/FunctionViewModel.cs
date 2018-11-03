using System;
using System.Reactive.Disposables;
using System.Windows.Input;
using Acr.UserDialogs;
using Plugin.Notifications;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Xamarin.Forms;


namespace Samples
{
    public class FunctionViewModel : ViewModel
    {
        public FunctionViewModel(INotificationManager notificationManager, IUserDialogs dialogs)
        {
            this.WhenAnyValue(x => x.Badge)
                .Subscribe(x => notificationManager.Badge = x)
                .DisposeWith(this.DeactivateWith);

            this.Vibrate = ReactiveCommand.Create(() => notificationManager.Vibrate());
            this.SendTest = ReactiveCommand.CreateFromTask(() => notificationManager.Send(new NotificationRequest
            {
                Title = "Hello",
                Message = "This is a test message"
            }));
            this.PermissionCheck = ReactiveCommand.CreateFromTask(async () =>
            {
                var result = await notificationManager.RequestPermission();
                dialogs.Toast("Permission Check Result: " + result);
            });
        }


        public ICommand Vibrate { get; }
        public ICommand SendTest { get; }
        public ICommand PermissionCheck { get; }
        public bool IsIos => Device.RuntimePlatform == Device.iOS;
        [Reactive] public int Badge { get; set; }
    }
}
/*
 CrossNotifications.Current.Activated += async (sender, notification) =>
{
    Debug.WriteLine($"Notification Activated - {notification.Id} - {notification.Title}");
    await this.MainPage.DisplayAlert("Received Notification", notification.Message, "OK");
};
 */