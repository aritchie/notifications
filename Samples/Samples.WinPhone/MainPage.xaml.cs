using System;
using Acr.Notifications;
using Microsoft.Phone.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WinPhone;


namespace Samples.WinPhone {

    public partial class MainPage : FormsApplicationPage {

        public MainPage() {
            this.InitializeComponent();
            this.SupportedOrientations = SupportedPageOrientation.PortraitOrLandscape;

            Notifications.Init();
            Forms.Init();
            this.LoadApplication(new Samples.App());
        }
    }
}
