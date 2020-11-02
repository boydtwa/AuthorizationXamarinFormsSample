using AuthXamSam.Views;
using AuthXamSam.Models;
using AuthXamSam.Services;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Xamarin.Forms;
using Xamarin.Essentials;
using GalaSoft.MvvmLight.Ioc;

namespace AuthXamSam.ViewModels
{
    public class LoginViewModel : ViewModelBase, ILoginViewModel
    {
        private readonly IMicrosoftAuthService microsoftAuthService;
        private bool isLoading;
        private User user;
        private readonly IMessaging messenger;
        private string buttonText;


        public ICommand LoginCommand { get; }

        public ICommand ButtonTextCommand { get; }

        public bool IsLoading
        {
            get { return isLoading; }
            set { Set(ref isLoading, value); }
        }

        public User User
        {
            get { return user; }
            set { Set(ref user, value); }
        }

        public string ButtonText
        {
            get { return buttonText; }
            set { Set(ref buttonText, value); }
        }

        public LoginViewModel(IMicrosoftAuthService MicrososoftAuthService)
        {
            microsoftAuthService = MicrososoftAuthService;
            messenger = SimpleIoc.Default.GetInstance<IMessaging>();
            LoginCommand = new RelayCommand(async () => await SignInAsync());
            ButtonTextCommand = new RelayCommand(async () => await SetButtonText());
            ButtonTextCommand.Execute(null);
        }

        public async Task SetButtonText()
        {
            var isAutheticated = await SecureStorage.GetAsync("tokenAuthXamSam");
            ButtonText = isAutheticated != null ? "Logout" : "Login";
        }

        public async Task SignInAsync()
        {
            try
            {
                var storedToken = await SecureStorage.GetAsync("tokenAuthXamSam");
                if (storedToken != null)
                {
                    await SignOutAsync();
                    return;
                }
                IsLoading = true;
                User = await microsoftAuthService.OnSignInAsync();
                ButtonTextCommand.Execute(null);
                IsLoading = false;
                App.Current.MainPage = new AppShell();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                await messenger.ShowMessageAsync("Authentication Error", $"Failed to Sign In. Message: {ex.Message}");
            }
        }

        public async Task SignOutAsync()
        {
            Xamarin.Essentials.SecureStorage.Remove("tokenAuthXamSam");
            try
            {
                this.IsLoading = true;
                await microsoftAuthService.OnSignOutAsync();
                ButtonTextCommand.Execute(null);
                user = null;
                this.IsLoading = false;
                App.Current.MainPage = new LoginPage();
            }
            catch (Exception ex)
            {
                // Manage the exception as you need, you can display an error message using a popup.
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                await messenger.ShowMessageAsync("Authentication Error", $"Failed to Log Out. Message: {ex.Message}");
            }
        }
    }
}
