using GalaSoft.MvvmLight.Ioc;
using AuthXamSam.Models;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace AuthXamSam.Services
{
    public class WineStore : IWineStore
    {
        private HttpClient _client;
        public string CellarBottleDetailsUri { get; private set; }
        public string CellarSummaryUri { get; private set; }
        public string CellarSummaryBottlesUri { get; private set; }

        public HttpClient Client
        {
            get => _client;
            set => _client = value;
        }

        private string Token { get; set; }

        [PreferredConstructor]
        public WineStore()
        {
            _client = new HttpClient();
            LoadServiceUrls();
        }

        public WineStore(HttpClient WebClient)
        {
            _client = WebClient;
            LoadServiceUrls();
        }

        public async Task<ObservableCollection<CellarSummaryModel>> GetCellarsAsync(bool ForceRefresh = false, string BearerTokenString = null, HttpRequestMessage HttpMessage = null)
        {
            var listofCellars = new ObservableCollection<CellarSummaryModel>();
            Token = BearerTokenString??SecureStorage.GetAsync("token").Result;
            HttpMessage = HttpMessage?? new HttpRequestMessage(HttpMethod.Get, CellarSummaryUri);
            HttpMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);



            var response = await _client.SendAsync(HttpMessage);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                listofCellars = JsonConvert.DeserializeObject<ObservableCollection<CellarSummaryModel>>(content);
            }
            /// TODO: handle unauthorized response

            return listofCellars;
        }
        private void LoadServiceUrls()
        {
            CellarBottleDetailsUri = "https://api.mywinedb.org/CellarBottleDetails";
            CellarSummaryUri = "https://api.mywinedb.org/CellarList";
            CellarSummaryBottlesUri = "https://api.mywinedb.org/CellarBottleSummaries";
        }
    }
}
