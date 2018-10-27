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

            this.Create = ReactiveCommand.CreateFromTask(async () =>
            {
                await notificationManager.Send(new Notification
                {
                    Title = "",
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
        }


        public ICommand Create { get; }
        [Reactive] public string Title { get; set; }
        [Reactive] public string Message { get; set; }
        [Reactive] public DateTime Date { get; set; }
        [Reactive] public TimeSpan Time { get; set; }
    }
}
