using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthXamSam.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AuthXamSam.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        private LoginViewModel viewModel;
        public LoginPage()
        {
            InitializeComponent();
            this.BindingContext = viewModel = SimpleIoc.Default.GetInstance<LoginViewModel>(); 
        }
    }
}