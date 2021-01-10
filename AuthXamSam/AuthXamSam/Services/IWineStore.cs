using AuthXamSam.Models;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;

namespace AuthXamSam.Services
{
    public interface IWineStore
    {
        HttpClient Client { get; set; }
        Task<ObservableCollection<CellarSummaryModel>> GetCellarsAsync(string BearerTokenString = null, HttpRequestMessage HttpMessage = null);
    }
}
