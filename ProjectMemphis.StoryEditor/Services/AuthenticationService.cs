using System;
using System.Net.Http;
using Windows.Data.Json;
using Windows.Security.Authentication.Web.Core;
using Windows.Security.Credentials;
using Windows.UI.ApplicationSettings;

namespace ProjectMemphis.StoryEditor.Services
{
    internal class AuthenticationService : IAuthenticationService
    {
        public string Name { get; set; }
        public Uri Picture { get;set; }

        public event EventHandler<SignInState> SignInChanged;

        public void SignInUser()
        {
            var accountPicker = AccountsSettingsPane.GetForCurrentView();
            accountPicker.AccountCommandsRequested += BuildPaneAsync;
            
            AccountsSettingsPane.Show();
        }

        private async void BuildPaneAsync(AccountsSettingsPane s, AccountsSettingsPaneCommandsRequestedEventArgs e)
        {
            var deferral = e.GetDeferral();

            WebAccountProvider msaProvider = await WebAuthenticationCoreManager.FindAccountProviderAsync("https://login.microsoft.com", "consumers");

            var command = new WebAccountProviderCommand(msaProvider, GetMsaTokenAsync);
            e.WebAccountProviderCommands.Add(command);

            deferral.Complete();
        }

        private async void GetMsaTokenAsync(WebAccountProviderCommand command)
        {
            WebTokenRequest request = new WebTokenRequest(command.WebAccountProvider, "wl.basic");
            WebTokenRequestResult result = await WebAuthenticationCoreManager.RequestTokenAsync(request);

            if (result.ResponseStatus == WebTokenRequestStatus.Success)
            {
                string token = result.ResponseData[0].Token;

                var restApi = new Uri(@"https://apis.live.net/v5.0/me?access_token=" + token);
                Picture = new Uri(@"https://apis.live.net/v5.0/me/picture?access_token=" + token);
                using (var client = new HttpClient())
                {
                    var infoResult = await client.GetAsync(restApi);
                    string content = await infoResult.Content.ReadAsStringAsync();

                    var jsonObject = JsonObject.Parse(content);
                    jsonObject["id"].GetString();
                    Name = jsonObject["name"].GetString();

                    SignInChanged?.Invoke(this, SignInState.Succeeded);
                }
            }
            else
            {
                SignInChanged?.Invoke(this, SignInState.Failed);
            }

            

        }
    }
}
