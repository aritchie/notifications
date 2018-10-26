using System;
using Acr.UserDialogs;
using Autofac;
using Plugin.Notifications;
using Prism;
using Prism.Autofac;
using Prism.Ioc;
using Prism.Mvvm;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation (XamlCompilationOptions.Compile)]


namespace Samples
{
	public partial class App : PrismApplication
	{
		public App() : this(null) {}
		public App(IPlatformInitializer initializer) : base(initializer)
		{
			this.InitializeComponent();
		}


	    protected override async void OnInitialized()
	    {
	        ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver(viewType =>
	        {
	            var viewModelTypeName = viewType.FullName.Replace("Page", "ViewModel");
	            var viewModelType = Type.GetType(viewModelTypeName);
	            return viewModelType;
	        });
            await this.NavigationService.NavigateAsync("NavigationPage/MainPage");
	    }


	    protected override void RegisterTypes(IContainerRegistry containerRegistry)
	    {
            containerRegistry.RegisterForNavigation<NavigationPage>();
		    
	        containerRegistry.RegisterForNavigation<CalendarTriggerPage>();
		    containerRegistry.RegisterForNavigation<FunctionPage>();
		    containerRegistry.RegisterForNavigation<LocationTriggerPage>();
		    containerRegistry.RegisterForNavigation<PendingListPage>();
		    containerRegistry.RegisterForNavigation<TimeIntervalTriggerPage>();
        }


	    protected override IContainerExtension CreateContainerExtension()
	    {
	        var builder = new ContainerBuilder();
		    builder.Register(_ => UserDialogs.Instance).As<IUserDialogs>().SingleInstance();
		    builder.RegisterType<GlobalExceptionHandler>().As<IStartable>().SingleInstance();
		    builder.Register(_ => CrossNotifications.Current).As<INotificationManager>().SingleInstance();
	        return new AutofacContainerExtension(builder);
        }
    }
}