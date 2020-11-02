using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AuthXamSam.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CellarPage : ContentPage
    {
        public CellarPage()
        {
            InitializeComponent();
            busyIndicator.HeightRequest = 150;
            view.LoadCellarsList.ExecuteAsync();
        }

        private void cbxCellers_SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {

        }
        private void busyIndicator_BindingContextChanged(object sender, EventArgs e)
        {
            if (IsBusy)
            {
                busyIndicator.HeightRequest = 150;
                busyIndicator.IsVisible = true;
            }
            else
            {
                busyIndicator.HeightRequest = 0;
                busyIndicator.IsVisible = false;
            }
        }
    }
}