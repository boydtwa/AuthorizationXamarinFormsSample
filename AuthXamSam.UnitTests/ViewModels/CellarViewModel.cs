using AuthXamSam.Models;
using AuthXamSam.Services;
using Moq;
using NUnit.Framework;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;

namespace AuthXamSam.UnitTests.ViewModels
{
    [TestFixture]
    public class CellarViewModel
    {
        protected Mock<HttpClient> MockHttpClient { get; private set; }
        protected Mock<IWineStore> MockWineStore { get; private set; }

        [SetUp]
        public void SetUpTest()
        {
            MockHttpClient = new Mock<HttpClient>();
            MockWineStore = new Mock<IWineStore>();
        }

        [Test]
        public async Task PopulateCellarListAsync_ReturnsACellarInList_Success()
        {
            MockWineStore.Setup(c => c.GetCellarsAsync(false,null,null)).ReturnsAsync(TestValues.CellarSummaryModelListObject);

            var sut = new AuthXamSam.ViewModels.CellarViewModel(false, MockWineStore.Object);
            await sut.PopulateCellarListAsync();

            Assert.AreEqual(1,sut.CellarListItems.Count);
        }

        [Test]
        public async Task PopulateCellarListAsync_ReturnsEmptyCellarList_Success()
        {
            MockWineStore.Setup(c => c.GetCellarsAsync(false, null, null)).ReturnsAsync(new ObservableCollection<CellarSummaryModel>());
            var sut = new AuthXamSam.ViewModels.CellarViewModel(false, MockWineStore.Object);
            await sut.PopulateCellarListAsync();

            Assert.AreEqual(0,sut.CellarListItems.Count);
        }

        [Test]
        public async Task PopulateCellarListAsync_ThrowsException()
        {
            var sut = new AuthXamSam.ViewModels.CellarViewModel(true, MockWineStore.Object);
            await sut.PopulateCellarListAsync();
            Assert.AreEqual("Exception Occurred", sut.SelectedCellarListItem.Text);
        }
    }
}
