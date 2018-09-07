using System;
using Prism.AppModel;
using ReactiveUI;


namespace Samples
{
    public class ViewModel : ReactiveObject, IPageLifecycleAware
    {
        public virtual void OnAppearing()
        {
        }


        public virtual void OnDisappearing()
        {
        }
    }
}
