using System.Net.Http.Headers;
using Microsoft.Extensions.Options;
using ODPC.Authentication;

namespace ODPC.Apis.Odrc
{
    public interface IOrdcClientFactory
    {
        HttpClient Create(OdpUser user, string handeling);
    }

    public class OrdcClientFactory(IHttpClientFactory httpClientFactory, IOptions<OdrcConfig> options) : IOrdcClientFactory
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
        private readonly IOptions<OdrcConfig> _options = options;

        public HttpClient Create(OdpUser user, string handeling)
        {
            var config = _options.Value;
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new(config.BaseUrl);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", config.ApiKey); //dit doet nog niets. nog niet geimplementeerd aan odrc kant. nb nog niet duidelijk of dit de juiste manier zal zijn voor het meesturen van het token
            client.DefaultRequestHeaders.Add("Audit-User-Id", user.Id); //dit doet nog niets. nog niet geimplementeerd aan odrc kant
            client.DefaultRequestHeaders.Add("Audit-User-Display-Name", user.FullName); //dit doet nog niets. nog niet geimplementeerd aan odrc kant
            client.DefaultRequestHeaders.Add("Audit-Toelichting", handeling); //dit doet nog niets. nog niet geimplementeerd aan odrc kant
            return client;
        }

    }
}
