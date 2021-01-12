using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using AuthXamSam.Models;
using AuthXamSam.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using Xamarin.Forms;

namespace AuthXamSam.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private bool isLoading;
        private IMessaging messenger;
        private IMicrosoftAuthService microsoftAuthService;
        private string signInOutText;
        private User user;

        [PreferredConstructor]
        public LoginViewModel()
        {
            InitializeViewModel(SimpleIoc.Default.GetInstance<IMicrosoftAuthService>(),
                SimpleIoc.Default.GetInstance<IMessaging>());
        }

        public LoginViewModel(IMicrosoftAuthService MicrosoftAuthService, IMessaging Messenger = null,
            string TestExceptionTestMsg = null)
        {
#if DEBUG
            InitializeViewModel(MicrosoftAuthService,Messenger,TestExceptionTestMsg);
#else
            InitializeViewModel(SimpleIoc.Default.GetInstance<IMicrosoftAuthService>(), SimpleIoc.Default.GetInstance<IMessaging>(), null);
#endif
        }

        public ICommand SignInOutCommand { get; private set; }

        public Command LoginCommand { get; }

        public string ExceptionTestMsg { get; set; }

        public string SignInOutText
        {
            get => signInOutText;
            set => Set(ref signInOutText, value);
        }

        public bool IsLoading
        {
            get => isLoading;
            set => Set(ref isLoading, value);
        }

        public User User
        {
            get => user;
            set => Set(ref user, value);
        }

        private void InitializeViewModel(IMicrosoftAuthService MicrosoftAuthService, IMessaging Messenger = null,
            string TestExceptionTestMsg = null)
        {
            microsoftAuthService = MicrosoftAuthService;
            messenger = Messenger ?? SimpleIoc.Default.GetInstance<IMessaging>();
            SignInOutCommand = new RelayCommand(async () => await SignInOutAsync());
            signInOutText = "Sign In";
            ExceptionTestMsg = TestExceptionTestMsg ?? string.Empty;
        }

        public async Task SignInOutAsync()
        {
            if (SignInOutText == "Sign In")
                await SignInAsync();
            else
                await SignOutAsync();
        }

        public async Task SignInAsync()
        {
            try
            {
                IsLoading = true;
                User = await microsoftAuthService.OnSignInAsync();
                if (User.DisplayName != "Not Authenticated")
                {
                    if (User.DisplayName == "Testing Exception") throw new Exception("For Testing Sign In Exception");
                    IsLoading = false;
                    SignInOutText = "Sign Out";
                    Application.Current.MainPage = new AppShell();
                }
                else
                {
                    await messenger.ShowMessageAsync("Authentication Error",
                        "User Name and or Password are not valid. OR you are not authorized to use this application");
                    IsLoading = false;
                    //message Center toast for Not Autheticated
                }
            }
            catch (Exception ex)
            {
                // Manage the exception as you need, you can display an error message using a popup.
                Debug.WriteLine(ex.ToString());
                ExceptionTestMsg = ex.Message;
            }
        }

        public async Task SignOutAsync()
        {
            try
            {
                if (ExceptionTestMsg == "Testing Exception") throw new Exception("For Testing Sign In Exception");
                IsLoading = true;
                await microsoftAuthService.OnSignOutAsync();
                user = null;
                SignInOutText = "Sign In";
                IsLoading = false;
            }
            catch (Exception ex)
            {
                // Manage the exception as you need, you can display an error message using a popup.
                Debug.WriteLine(ex.ToString());
                ExceptionTestMsg = ex.Message;
            }
        }
    }
}