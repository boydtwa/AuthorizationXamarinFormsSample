using AuthXamSam.Models;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using Xamarin.Essentials;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using System.Linq;

namespace AuthXamSam.Services
{
    public class MicrosoftAuthService : IMicrosoftAuthService
    {
        private readonly string ClientID = "<application ID for AuthXamSamApplication in Azure AD>";
        private readonly string[] Scopes = { $"<application ID in Azure Function API>/user_impersonation>" };

        public User CurrentUser { get; private set; }

        /// <summary>
        /// This object is used to know where to display the authentication activity (for Android) or page.
        /// </summary>
        public static object ParentWindow { get; set; }

        public static IPublicClientApplication PCA = null;

        public void Initialize()
        {
            PCA = PublicClientApplicationBuilder.Create(ClientID)
                .WithRedirectUri($"msal{ClientID}://auth")
                .Build();
        }

        public async Task<User> OnSignInAsync()
        {
            var accounts = await PCA.GetAccountsAsync();
            try
            {
                try
                {
                    var firstAccount = accounts.FirstOrDefault();
                    var authResult = await PCA.AcquireTokenSilent(Scopes, firstAccount).ExecuteAsync();

                    string accessToken = authResult.AccessToken;
                    await Xamarin.Essentials.SecureStorage.SetAsync("tokenAuthXamSam", accessToken);
                    string idToken = authResult.IdToken;
                    RefreshUserDataAsync(authResult?.AccessToken);
                }
                catch (MsalUiRequiredException ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                    // the user was not already connected.
                    try
                    {
                        var parentWindow = App.ParentWindow;

                        var authResult = await PCA.AcquireTokenInteractive(Scopes)
                            .WithUseEmbeddedWebView(true) //Added this ...
                            .WithParentActivityOrWindow(parentWindow)
                            .ExecuteAsync();

                        string accessToken = authResult.AccessToken;
                        await Xamarin.Essentials.SecureStorage.SetAsync("tokenAuthXamSam", accessToken);
                        string idToken = authResult.IdToken;

                        RefreshUserDataAsync(authResult?.AccessToken);
                    }
                    catch (Exception ex2)
                    {
                        // Manage the exception with a logger as you need
                        System.Diagnostics.Debug.WriteLine(ex2.ToString());
                        return CurrentUser;
                    }
                }
                catch (Exception e3)
                {
                    var msg = e3.Message;
                    return CurrentUser;
                }
            }
            catch (Exception e4)
            {
                var msg = e4.Message;
                return CurrentUser;
            }

            return CurrentUser ?? new User() { DisplayName = "Not Autheticated" };
        }

        /// <summary>
        /// Sign out with your Microsoft account.
        /// </summary>
        public async Task OnSignOutAsync()
        {
            var accounts = await PCA.GetAccountsAsync();
            try
            {
                Xamarin.Essentials.SecureStorage.Remove("tokenAuthXamSam");
                while (accounts.Any())
                {
                    await PCA.RemoveAsync(accounts.FirstOrDefault());
                    accounts = await PCA.GetAccountsAsync();
                }
            }
            catch (Exception ex)
            {
                // Manage the exception with a logger as you need
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// Refresh user date from the Graph api.
        /// </summary>
        /// <param name="token">The user access token.</param>
        /// <returns>The current user with his associated informations.</returns>
        private void RefreshUserDataAsync(string token)
        {
            //figure out how to read access token for user meta data
            //Hard wire it for now
            CurrentUser = new User()
            {
                DisplayName = "<display name>",
                GivenName = "<first name>",
                Surname = "<last name>",
                Email = "<email address>",
                UserPrincipalName = "<user principle name>",
                Token = token
            };


        }
    }
}
