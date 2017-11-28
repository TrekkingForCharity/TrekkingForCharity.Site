using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace TrekkingForCharity.Site.Web.Integrations.Doorbell
{
    public interface IDoorbellClient
    {
        Task<bool> Submit(DoorbellMessage doorbellMessage);
    }
    public class DoorbellClient : IDoorbellClient
    {
        private readonly DoorbellSettings _doorbellSettings;
        private readonly HttpClient _httpClient;
        public DoorbellClient(IOptions<DoorbellSettings> doorbellSettings)
        {
            _doorbellSettings = doorbellSettings.Value;
            _httpClient = new HttpClient{
                BaseAddress = new Uri(_doorbellSettings.Url)
            };
        }

        public async Task<bool> Submit(DoorbellMessage doorbellMessage)
        {
            var dataAsString = JsonConvert.SerializeObject(doorbellMessage);
            var content = new StringContent(dataAsString);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await _httpClient.PostAsync($"/applications/{_doorbellSettings.ApplicationId}/submit?key={_doorbellSettings.Key}", content);
            return response.IsSuccessStatusCode;
        }
    }
}