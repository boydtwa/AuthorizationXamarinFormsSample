using AuthXamSam.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace AuthXamSam.Services
{
    public class WineStore : IWineStore
    {
        HttpClient client;
        public string cellarSummaryUri { get; }

        public WineStore()
        {
            client = new HttpClient();

            cellarSummaryUri = "<Uri to API that returns the Cellar Summary>";
        }

        public async Task<ObservableCollection<CellarSummaryModel>> GetCellarsAsync(bool forceRefresh = false)
        {
            var listofCellars = new ObservableCollection<CellarSummaryModel>();
            var token = SecureStorage.GetAsync("tokenAuthXamSam").Result;
            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, cellarSummaryUri);
            message.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await client.SendAsync(message);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                listofCellars = JsonConvert.DeserializeObject<ObservableCollection<CellarSummaryModel>>(content);
            }
            /// TODO: handle unauthorized response

            return listofCellars;
        }
    }
}
