using System;
using System.Collections.Generic;
using AuthXamSam.ViewModels;
using AuthXamSam.Views;
using Xamarin.Forms;

namespace AuthXamSam
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
