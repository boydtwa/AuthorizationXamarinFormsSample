using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace AuthXamSam.Services
{
    public class Messaging : IMessaging
    {
        public async Task ShowMessageAsync(string Title, string Message)
        {
            await Xamarin.Forms.Application.Current.MainPage.DisplayAlert(Title, Message, "OK");
        }
    }
}
