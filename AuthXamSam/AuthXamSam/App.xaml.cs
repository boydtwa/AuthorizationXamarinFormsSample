using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using AuthXamSam.Services;
using AuthXamSam.Views;
using AuthXamSam.ViewModels;
using GalaSoft.MvvmLight.Ioc;

namespace AuthXamSam
{
    public partial class App : Application
    {
        public static object ParentWindow { get; set; }
        public App()
        {
            /// Get a trial licence for Syncfusion at www.syncfusion.com
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("<Syncfusion Licenence for Xamarin UI Componenets>");
            InitializeComponent();
            //register Page Routes
            Routing.RegisterRoute(nameof(AboutPage), typeof(AboutPage));
            SimpleIoc.Default.Register<IWineStore, WineStore>();
            SimpleIoc.Default.Register<IMicrosoftAuthService,MicrosoftAuthService>();
            SimpleIoc.Default.Register<IMessaging, Messaging>();
            SimpleIoc.Default.Register<LoginViewModel>();
            SimpleIoc.Default.GetInstance<IMicrosoftAuthService>().Initialize();
            MainPage = new LoginPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
