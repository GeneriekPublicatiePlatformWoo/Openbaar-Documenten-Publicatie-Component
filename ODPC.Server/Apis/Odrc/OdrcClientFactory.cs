using System.Net.Http.Headers;
using ODPC.Authentication;

namespace ODPC.Apis.Odrc
{
    public interface IOdrcClientFactory
    {
        HttpClient Create(string handeling);
    }

    public class OdrcClientFactory(IHttpClientFactory httpClientFactory, IConfiguration config, OdpcUser user) : IOdrcClientFactory
    {
        public HttpClient Create(string? handeling)
        {
            var client = httpClientFactory.CreateClient();
            client.BaseAddress = new(config["ODRC_BASE_URL"]!);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", config["ODRC_API_KEY"]);
            client.DefaultRequestHeaders.Add("Audit-User-ID", user.Id);
            client.DefaultRequestHeaders.Add("Audit-User-Representation", user.FullName);
            client.DefaultRequestHeaders.Add("Audit-Remarks", handeling);
            return client;
        }
    }
}
