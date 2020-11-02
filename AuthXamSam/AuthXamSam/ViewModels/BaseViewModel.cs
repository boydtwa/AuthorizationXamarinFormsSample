using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

using Xamarin.Forms;

using AuthXamSam.Models;
using AuthXamSam.Services;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Command;
using System.Threading.Tasks;
using System.Net.Http;

namespace AuthXamSam.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public IWineStore WineStore { get; }

        public ICommand BaseSignOutCommand { get; }


        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        string title = string.Empty;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        public BaseViewModel()
        {
            WineStore = SimpleIoc.Default.GetInstance<IWineStore>();
            BaseSignOutCommand = new RelayCommand(async () => await BaseSignOutAsync());
        }

        public async Task BaseSignOutAsync()
        {
            var answer = await App.Current.MainPage.DisplayAlert("Log Out Confirmation", "Are you sure you want to Log Out?", "Yes", "No");
        }
    }
}
