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
        public async Task<IActionResult> Get(
            string apiVersion,
            CancellationToken token,
            [FromQuery] string? page = "1",
            [FromQuery] string? sorteer = "-registratiedatum",
            [FromQuery] string? search = "",
            [FromQuery] string? registratiedatumVanaf = "",
            [FromQuery] string? registratiedatumTot = "")
        {
            // publicaties ophalen uit het ODRC
            using var client = clientFactory.Create("Publicaties ophalen");

            var parameters = new Dictionary<string, string?>
            {
                { "eigenaar", WebUtility.UrlEncode(user.Id) },
                { "page", page },
                { "sorteer", sorteer },
                { "search", search },
                { "registratiedatumVanaf", registratiedatumVanaf },
                { "registratiedatumTot", registratiedatumTot }
            };

            var queryString = UrlHelper.BuildQueryString(parameters);

            var url = $"/api/{apiVersion}/publicaties?{queryString}";

            var json = await client.GetFromJsonAsync<PagedResponseModel<JsonNode>>(url, token);

            return Ok(json);
        }
    }
}
