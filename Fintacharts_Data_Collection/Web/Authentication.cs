using Microsoft.OpenApi.Expressions;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Timers;

namespace Fintacharts_Data_Collection.Web
{
    public class Authentication
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        private string access_tocken;
        private string refresh_tocken;

        private System.Timers.Timer aTimer;

        bool access_token_expired = true;
        //bool refresh_token_expired = true;
        //Couldn't find the way to receive new tocken with refresh token

        async public Task<string> Access_Tocken()
        {
            if (access_token_expired == true)
            {
                await GetNewTockensAsync();
            }
            return access_tocken;
        }

        public Authentication(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            aTimer = new System.Timers.Timer(1800000);
        }

        async public Task GetNewTockensAsync()
        {
            var httpClient = _httpClientFactory.CreateClient("Fintacharts");
            var parameters = new Dictionary<string, string>();

            parameters.Add("grant_type", "password");
            parameters.Add("client_id", "app-cli");
            parameters.Add("username", _configuration["Web:USERNAME"]);
            parameters.Add("password", _configuration["Web:PASSWORD"]);

            using HttpResponseMessage response = await httpClient.PostAsync("identity/realms/fintatech/protocol/openid-connect/token", new FormUrlEncodedContent(parameters));
            if(response.IsSuccessStatusCode)
            {
                
                var content = await response.Content.ReadAsStringAsync();
                dynamic document = JsonConvert.DeserializeObject(content);
                //var document = JsonDocument.Parse(content);

                access_tocken = document.access_token;
                refresh_tocken = document.refresh_tocken;

                access_token_expired = false;
                SetTimer();
            }
            else
            {
                throw new Exception(response.Content.ToString());
            }
        }

        //In case of token expiration, new tocken would be received with refresh tocken, but no functionality for that was found
        //async public Task RefreshTocken()
        //{

        //}
        private void SetTimer()
        {
            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += OnTimedEvent;
            aTimer.Enabled = true;
        }

        private async void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            Console.WriteLine("The Elapsed event was raised at {0:HH:mm:ss.fff}",
                              e.SignalTime);
            access_token_expired = true;
        }
    }
}
