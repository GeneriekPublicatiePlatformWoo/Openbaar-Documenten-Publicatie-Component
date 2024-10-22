using System.Net.Http.Headers;
using ODPC.Authentication;

namespace ODPC.Apis.Odrc
{
    public interface IOdrcClientFactory
    {
        HttpClient Create(OdpUser user, string handeling);
    }

    public class OdrcClientFactory(IHttpClientFactory httpClientFactory, IConfiguration config) : IOdrcClientFactory
    {
        public HttpClient Create(OdpUser user, string? handeling)
        {
            var client = httpClientFactory.CreateClient();
            client.BaseAddress = new(config["ODRC_BASE_URL"]!);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", config["ODRC_API_KEY"]); //dit doet nog niets. nog niet geimplementeerd aan odrc kant. nb nog niet duidelijk of dit de juiste manier zal zijn voor het meesturen van het token
            client.DefaultRequestHeaders.Add("Audit-User-ID", user.Id);
            client.DefaultRequestHeaders.Add("Audit-User-Representation", user.FullName);
            client.DefaultRequestHeaders.Add("Audit-Remarks", handeling);
            return client;
        }
    }
}
