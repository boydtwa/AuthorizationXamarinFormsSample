using AuthXamSam.Models;
using MyWineDb.Mobile.UnitTests;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;


namespace AuthXamSam.UnitTests.Services
{
    [TestFixture]
    public class WineStore
    {
        private AuthXamSam.Services.WineStore SystemUnderTest { get; set; }

        [SetUp]
        public void WineStoreSetup()
        {

        }

        [TearDown]
        public void WineStoreTearDown()
        {
            SystemUnderTest = null;
        }

        [Test]
        public async Task GetCellarsAsync_ReturnsValidResponse()
        {
            var fakeResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(TestValues.CellarSummaryModelListJson, Encoding.UTF8, "application/json")
            };
            var fakeHandler = new FakeHttpMessageHandler(fakeResponse);
            var mockHttpClient = new HttpClient(fakeHandler) {BaseAddress = new Uri("http://localhost")};
            SystemUnderTest = new AuthXamSam.Services.WineStore(mockHttpClient);

            var result = await Task.FromResult(SystemUnderTest.GetCellarsAsync(false, "foobarToken"));
            var expectedResult =
                JsonConvert.DeserializeObject<ObservableCollection<CellarSummaryModel>>(TestValues
                    .CellarSummaryModelListJson);
            Assert.AreEqual(expectedResult.Count, result.Result.Count);
            Assert.AreEqual(expectedResult[0].CellarId.PartitionKey, result.Result[0].CellarId.PartitionKey);
            Assert.AreEqual(expectedResult[0].CellarId.RowKey, result.Result[0].CellarId.RowKey);
            Assert.AreEqual(expectedResult[0].CellarId.TimeStamp, result.Result[0].CellarId.TimeStamp);
            Assert.AreEqual(expectedResult[0].Capacity, result.Result[0].Capacity);
            Assert.AreEqual(expectedResult[0].Description, result.Result[0].Description);
            Assert.AreEqual(expectedResult[0].Value, result.Result[0].Value);
        }

        [Test]
        public async Task GetCellarsAsync_ReturnsEmptyCollectionWhenResponseIsNotValid()
        {
            var fakeResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.ServiceUnavailable,
            };
            var fakeHandler = new FakeHttpMessageHandler(fakeResponse);
            var fakeHttpClient = new HttpClient(fakeHandler) {BaseAddress = new Uri("http://localhost")};
            SystemUnderTest = new AuthXamSam.Services.WineStore(fakeHttpClient);
            var response = await Task.FromResult(SystemUnderTest.GetCellarsAsync(false, "foobarToken"));
            Assert.AreEqual(0, response.Result.Count);
        }

    }
}
