using NUnit;
using NUnit.Framework;
using Moq;
using AuthXamSam.Models;
using AuthXamSam.Services;
using AuthXamSam.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.Common.Utilities;
using Newtonsoft.Json;

namespace AuthXamSam.UnitTests.ViewModels
{
    [TestFixture]
    public class LoginViewModelTests
    {

        [Test]
        public async Task SignInAsync_SignIn_Succeeds()
        {
            var user = new User() {DisplayName = "foo"};
            var mockAuthService = new Mock<IMicrosoftAuthService>();
                mockAuthService.Setup(s =>s.OnSignInAsync()).ReturnsAsync(user);

            var sut = new AuthXamSam.ViewModels.LoginViewModel(mockAuthService.Object, new Mock<IMessaging>().Object);
            await Task.FromResult(sut.SignInAsync());

            Assert.AreNotEqual("Not Authenticated",sut.User.DisplayName);
        }

        [Test]
        public async Task SignInAsync_SignIn_Fails()
        {
            var user = new User() {DisplayName = "Not Authenticated"};
            var mockAuthService = new Mock<IMicrosoftAuthService>();
            mockAuthService.Setup(s =>s.OnSignInAsync()).ReturnsAsync(user);

            var sut = new AuthXamSam.ViewModels.LoginViewModel(mockAuthService.Object, new Mock<IMessaging>().Object);
            await Task.FromResult(sut.SignInAsync());

            Assert.AreEqual("Not Authenticated",sut.User.DisplayName);
        }
        [Test]
        public async Task SignInAsync_SignIn_ThrowsException()
        {
            var user = new User() {DisplayName = "Testing Exception"};
            var mockAuthService = new Mock<IMicrosoftAuthService>();
            mockAuthService.Setup(s =>s.OnSignInAsync()).ReturnsAsync(user);

            var sut = new AuthXamSam.ViewModels.LoginViewModel(mockAuthService.Object, new Mock<IMessaging>().Object);
            await Task.FromResult(sut.SignInAsync());


            Assert.AreEqual("For Testing Sign In Exception", sut.ExceptionTestMsg);
        }

        [Test]
        public async Task SignOutAsync_Success()
        {
            var user = new User() {DisplayName = "foo"};
            var mockAuthService = new Mock<IMicrosoftAuthService>();
            mockAuthService.Setup(s =>s.OnSignInAsync()).ReturnsAsync(user);

            var sut = new AuthXamSam.ViewModels.LoginViewModel(mockAuthService.Object, new Mock<IMessaging>().Object);
            await Task.FromResult(sut.SignOutAsync());
            Assert.IsNull(sut.User);
            Assert.AreEqual("Sign In", sut.SignInOutText);
        }

        [Test]
        public async Task SignOutAsync_ThrowsException()
        {
            var user = new User() {DisplayName = "Testing Exception"};
            var mockAuthService = new Mock<IMicrosoftAuthService>();
            mockAuthService.Setup(s =>s.OnSignInAsync()).ReturnsAsync(user);

            var sut = new AuthXamSam.ViewModels.LoginViewModel(mockAuthService.Object, new Mock<IMessaging>().Object, "For Testing Sign In Exception");
            await Task.FromResult(sut.SignOutAsync());

            Assert.AreEqual("For Testing Sign In Exception", sut.ExceptionTestMsg);
        }

        [Test]
        public async Task SignInOutAsync_ExecutesSignInAsync()
        {
            var user = new User() {DisplayName = "foo"};
            var mockAuthService = new Mock<IMicrosoftAuthService>();
            mockAuthService.Setup(s =>s.OnSignInAsync()).ReturnsAsync(user);

            var sut = new AuthXamSam.ViewModels.LoginViewModel(mockAuthService.Object, new Mock<IMessaging>().Object);
            sut.SignInOutText = "Sign In";
            await Task.FromResult(sut.SignInAsync());

            Assert.AreNotEqual("Not Authenticated",sut.User.DisplayName);
        }

        [Test]
        public async Task SignInOutAsync_ExecutesSignOutAsync()
        {
            var user = new User() {DisplayName = "foo"};
            var mockAuthService = new Mock<IMicrosoftAuthService>();
            mockAuthService.Setup(s =>s.OnSignInAsync()).ReturnsAsync(user);

            var sut = new AuthXamSam.ViewModels.LoginViewModel(mockAuthService.Object, new Mock<IMessaging>().Object);
            sut.SignInOutText = "foobar";
            await Task.FromResult(sut.SignOutAsync());
            Assert.IsNull(sut.User);
            Assert.AreEqual("Sign In", sut.SignInOutText);
        }

    }
}
