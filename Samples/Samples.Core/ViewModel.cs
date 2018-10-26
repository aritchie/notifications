using System;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using Prism.Navigation;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;


namespace Samples
{
    public abstract class ViewModel : ReactiveObject,
                                      INavigatingAware,
                                      INavigatedAware,
                                      IDestructible,
                                      IConfirmNavigationAsync
    {
        CompositeDisposable deactivateWith;
        protected CompositeDisposable DeactivateWith
        {
            get
            {
                if (this.deactivateWith == null)
                    this.deactivateWith = new CompositeDisposable();

                return this.deactivateWith;
            }
        }

        protected CompositeDisposable DestroyWith { get; } = new CompositeDisposable();


        public virtual void OnNavigatingTo(INavigationParameters parameters)
        {
        }


        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {
            this.deactivateWith?.Dispose();
            this.deactivateWith = null;
        }


        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {
        }


        public virtual void Destroy()
        {
            this.DestroyWith?.Dispose();
        }


        public virtual Task<bool> CanNavigateAsync(INavigationParameters parameters) => Task.FromResult(true);
        [Reactive] public bool IsBusy { get; protected set; }
        [Reactive] public string Title { get; protected set; }
    }
}
