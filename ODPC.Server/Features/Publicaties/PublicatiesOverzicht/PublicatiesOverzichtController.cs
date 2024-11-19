using Microsoft.AspNetCore.Mvc;
using ODPC.Authentication;
using ODPC.Apis.Odrc;
using System.Text.Json.Nodes;
using System.Net;

namespace ODPC.Features.Publicaties.PublicatiesOverzicht
{
    [ApiController]
    public class PublicatiesOverzichtController(IOdrcClientFactory clientFactory, OdpcUser user) : ControllerBase
    {
        [HttpGet("api/{apiVersion}/publicaties")]
        public async Task<IActionResult> Get(string apiVersion, CancellationToken token, [FromQuery] string? page = "1", string? sorteer = "-registratiedatum", string? search = "", string? registratiedatumVanaf = "", string? registratiedatumTot = "")
        {
            // publicaties ophalen uit het ODRC
            using var client = clientFactory.Create("Publicaties ophalen");
            var url = "/api/" + apiVersion + "/publicaties?eigenaar=" + WebUtility.UrlEncode(user.Id) + "&page=" + page + "&sorteer=" + sorteer + "&search=" + search + "&registratiedatumVanaf=" + registratiedatumVanaf + "&registratiedatumTot=" + registratiedatumTot;

            var json = await client.GetFromJsonAsync<PagedResponseModel<JsonNode>>(url, token);

            return Ok(json);
        }
    }
}
