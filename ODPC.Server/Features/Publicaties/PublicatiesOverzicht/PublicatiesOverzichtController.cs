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
        [HttpGet("api/v1/publicaties")]
        public async Task<IActionResult> Get(CancellationToken token, [FromQuery] string? page = "1", string? sorteer = "-registratiedatum", string? search = "", string? registratiedatumVanaf = "", string? registratiedatumTot = "")
        {
            // publicaties ophalen uit het ODRC
            using var client = clientFactory.Create("Publicaties ophalen");
            var json = await client.GetFromJsonAsync<PagedResponseModel<JsonNode>>(
                "/api/v1/publicaties?eigenaar=" + WebUtility.UrlEncode(user.Id) + "&page=" + page + "&sorteer=" + sorteer + "&search=" + WebUtility.UrlEncode(search),
                token
            );

            if (json != null)
            {
                json.Previous = UrlHelper.GetPathAndQuery(json.Previous);
                json.Next = UrlHelper.GetPathAndQuery(json.Next);
            }

            return Ok(json);
        }
    }
}
