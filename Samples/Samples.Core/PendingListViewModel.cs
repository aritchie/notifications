using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using Plugin.Notifications;
using Prism.Navigation;
using ReactiveUI;
using ReactiveUI.Legacy;


namespace Samples
{
    public class PendingListViewModel : ViewModel
    {
        readonly INotificationManager notificationManager;
        readonly IUserDialogs dialogs;


        public PendingListViewModel(INotificationManager notificationManager, IUserDialogs dialogs)
        {
            this.notificationManager = notificationManager;
            this.dialogs = dialogs;
            this.CancelAll = ReactiveCommand.CreateFromTask(async () =>
            {
                var ok = await dialogs.ConfirmAsync("Cancel All Notifications?");
                if (ok)
                {
                    await this.notificationManager.CancelAll();
                    dialogs.Toast("All Notifications Cancelled");
                    await this.Load();
                }
            });
        }


        public ReactiveList<PendingItemViewModel> Pending { get; } = new ReactiveList<PendingItemViewModel>();
        public ICommand CancelAll { get; }
        public override async void OnNavigatedTo(INavigationParameters parameters) => await this.Load();


        async Task Load()
        {
            try
            {
                this.Pending.Clear();

                var items = await this.notificationManager.GetPendingNotifications();
                var vms = items.Select(x => new PendingItemViewModel(x)
                {
                    Cancel = ReactiveCommand.CreateFromTask(async () =>
                    {
                        await this.notificationManager.Cancel(x.Id);
                        this.dialogs.Toast("Notification Cancelled");
                        await this.Load();
                    })
                }).ToList();
                this.Pending.AddRange(vms);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                this.dialogs.Alert(ex.ToString());
            }
        }
    }


    public class PendingItemViewModel
    {
        readonly NotificationInfo notification;
        public PendingItemViewModel(NotificationInfo notification) => this.notification = notification;


        public string Title => this.notification.Request.Title;
        public string Message => this.notification.Request.Message;
        public ICommand Cancel { get; set; }
    }
}