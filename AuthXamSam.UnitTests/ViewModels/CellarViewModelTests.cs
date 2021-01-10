using System;
using AuthXamSam.Models;
using AuthXamSam.Services;
using Moq;
using NUnit.Framework;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using NUnit.Framework.Constraints;

namespace AuthXamSam.UnitTests.ViewModels
{
    [TestFixture]
    public class CellarViewModelTests
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
            MockWineStore.Setup(c => c.GetCellarsAsync(null,null)).ReturnsAsync(TestValues.CellarSummaryModelListObject);

            var sut = new AuthXamSam.ViewModels.CellarViewModel(MockWineStore.Object);
            await sut.PopulateCellarListAsync();

            Assert.AreEqual(1,sut.CellarListItems.Count);
        }

        [Test]
        public async Task PopulateCellarListAsync_ReturnsEmptyCellarList_Success()
        {
            MockWineStore.Setup(c => c.GetCellarsAsync( null, null)).ReturnsAsync(new ObservableCollection<CellarSummaryModel>());
            var sut = new AuthXamSam.ViewModels.CellarViewModel(MockWineStore.Object);
            await sut.PopulateCellarListAsync();

            Assert.AreEqual(0,sut.CellarListItems.Count);
        }

        [Test]
        public void PopulateCellarListAsync_ThrowsException()
        {
            MockWineStore.Setup(p => p.GetCellarsAsync(null,null) ).Throws<Exception>();
            var sut = new AuthXamSam.ViewModels.CellarViewModel(MockWineStore.Object);

            Assert.That(async ()=>await sut.PopulateCellarListAsync(),new ThrowsExceptionConstraint());
        }
    }
}
