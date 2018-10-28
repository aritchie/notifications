using System;
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
        public PendingListViewModel(INotificationManager notificationManager, IUserDialogs dialogs)
        {
            this.CancelAll = ReactiveCommand.CreateFromTask(async () => { });
        }


        public ReactiveList<PendingItemViewModel> Pending { get; } = new ReactiveList<PendingItemViewModel>();
        public ICommand CancelAll { get; }


        public override void OnNavigatedTo(INavigationParameters parameters)
        {
        }
    }


    public class PendingItemViewModel
    {

    }
}