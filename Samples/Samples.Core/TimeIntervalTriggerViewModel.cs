using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using Plugin.Notifications;
using ReactiveUI;


namespace Samples
{
    public class TimeIntervalTriggerViewModel : ViewModel
    {
        readonly INotificationManager notificationManager;
        readonly IUserDialogs dialogs;


        public TimeIntervalTriggerViewModel(INotificationManager notificationManager,
                                            IUserDialogs dialogs)
        {
            this.notificationManager = notificationManager;
            this.dialogs = dialogs;

            this.EveryMinute = this.Create("MINUTE", "I will get annoying fast", TimeSpan.FromMinutes(1));
            this.EveryHour = this.Create("HOUR", "hi - top of the hour to ya", TimeSpan.FromHours(1));
        }


        ICommand Create(string title, string msg, TimeSpan ts) => ReactiveCommand.CreateFromTask(async () =>
        {
            await this.notificationManager.Send(new NotificationRequest
            {
                Title = title,
                Message = msg,
                Trigger = new TimeIntervalNotificationTrigger(
                    ts,
                    true
                )
            });
            this.dialogs.Toast("Notification Interval Created");
        });


        public ICommand EveryMinute { get; }
        public ICommand EveryHour { get; }
    }
}
