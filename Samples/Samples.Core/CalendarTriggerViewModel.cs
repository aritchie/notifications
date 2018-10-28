using System;
using System.Windows.Input;
using Acr.UserDialogs;
using Plugin.Notifications;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;


namespace Samples
{
    public class CalendarTriggerViewModel : ViewModel
    {
        public CalendarTriggerViewModel(INotificationManager notificationManager, IUserDialogs dialogs)
        {
            this.Date = DateTime.Now.Date;
            this.Time = DateTime.Now.AddMinutes(10).TimeOfDay;

            this.CreateSpecific = ReactiveCommand.CreateFromTask(async () =>
            {
                var dt = this.Date.Date.Add(this.Time);
                await notificationManager.Send(new Notification
                {
                    Title = this.MessageTitle,
                    Message = this.Message,
                    Trigger = CalendarNotificationTrigger.CreateFromSpecificDateTime(dt),
                    Windows = new UwpOptions
                    {
                        UseLongDuration = true
                    },
                    Android = new AndroidOptions
                    {
                        Channel = "CalendarTriggers"
                    }
                });
                dialogs.Toast("Notification Created");
            },
            this.WhenAny(
                x => x.Date,
                x => x.Time,
                (dt, t) =>
                {
                    var date = dt.GetValue().Date;
                    if (dt.GetValue() > DateTime.Now.Date)
                        return true;

                    var ts = t.GetValue();
                    var newDate = date.Add(ts);
                    if (newDate > DateTime.Now)
                        return true;

                    return false;
                }
            ));

            this.Yearly = ReactiveCommand.CreateFromTask(() =>
                notificationManager.Send(new Notification
                {
                    Title = "Yearly",
                    Message = "Happy Birthday - Ya you old now",
                    Trigger = new CalendarNotificationTrigger(new DateParts
                    {
                        Month = 1,
                        Day = 30,
                        TimeOfDay = TimeSpan.FromHours(8)
                    }, true)
                })
            );

            this.Monthly = ReactiveCommand.CreateFromTask(() =>
                notificationManager.Send(new Notification
                {
                    Title = "Monthly - 1st of Month",
                    Message = "Pinch & Punch - First of the Month",
                    Trigger = new CalendarNotificationTrigger(new DateParts
                    {
                        DayOfWeek = DayOfWeek.Monday,
                        TimeOfDay = TimeSpan.FromHours(7)
                    }, true)
                })
            );

            this.Weekly = ReactiveCommand.CreateFromTask(() =>
                notificationManager.Send(new Notification
                {
                    Title = "Daily",
                    Message = "WORKTIME - Yay",
                    Trigger = new CalendarNotificationTrigger(new DateParts
                    {
                        DayOfWeek = DayOfWeek.Monday,
                        TimeOfDay = TimeSpan.FromHours(7)
                    }, true)
                })
            );
        }

        // TODO: days of week, weekdays,
        public ICommand Yearly { get; }
        public ICommand Monthly { get; }
        public ICommand Weekly { get; }

        public ICommand CreateSpecific { get; }
        [Reactive] public string MessageTitle { get; set; }
        [Reactive] public string Message { get; set; }
        [Reactive] public DateTime Date { get; set; }
        [Reactive] public TimeSpan Time { get; set; }
    }
}
