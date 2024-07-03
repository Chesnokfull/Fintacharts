using Fintacharts_Data_Collection.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics.Metrics;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Fintacharts_Data_Collection.Web
{
    public class GetInstruments
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly Authentication _authentication;

        public GetInstruments(IHttpClientFactory httpClientFactory, Authentication authentication)
        {
            _httpClientFactory = httpClientFactory;
            _authentication = authentication;
        }

        async public Task<List<Models.Instrument>> Get()
        {
            var httpClient = _httpClientFactory.CreateClient("Fintacharts");

            httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", await _authentication.Access_Tocken());

            using HttpResponseMessage response = await httpClient.GetAsync("/api/instruments/v1/instruments?provider=oanda&kind=forex");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                JObject o = JObject.Parse(content);

                List<Models.Instrument> result = new List<Models.Instrument>();
                foreach(var instrument in o["data"])
                {
                    List<Provider> providers = new List<Provider>();
                    Models.Instrument newInstrument = JsonConvert.DeserializeObject<Models.Instrument>(instrument.ToString());
                    foreach(var provider in instrument["mappings"])
                    {
                        Provider newProvider = new Provider();
                        newProvider = JsonConvert.DeserializeObject<Provider>(provider.First.ToString());
                        var pathElements = provider.Path.Split('.');
                        newProvider.Name = pathElements.Last();
                        newProvider.Instrument = newInstrument;
                        providers.Add(newProvider);
                    }
                    newInstrument.providers = providers;
                    result.Add(newInstrument);
                }
                
                return result;
            }
            else
            {
                throw new Exception(response.Content.ToString());
            }
        }
    }
}

