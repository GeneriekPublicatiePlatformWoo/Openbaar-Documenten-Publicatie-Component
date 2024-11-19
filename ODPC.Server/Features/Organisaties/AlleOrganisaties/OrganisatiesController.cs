using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Mvc;
using ODPC.Apis.Odrc;

namespace ODPC.Features.Organisaties.AlleOrganisaties
{
    [ApiController]
    public class OrganisatiesController(IOdrcClientFactory clientFactory) : ControllerBase
    {
        [HttpGet("api/{apiVersion}/organisaties")]
        public async Task<IActionResult> Get(string apiVersion, [FromQuery] string? page, CancellationToken token)
        {
            // organisaties ophalen uit het ODRC
            using var client = clientFactory.Create("Organisaties ophalen");
            var url = "/api/" + apiVersion + "/organisaties?page=" + page;

            var json = await client.GetFromJsonAsync<PagedResponseModel<JsonNode>>(url, token);

            if (json != null)
            {
                json.Previous = UrlHelper.GetPathAndQuery(json.Previous);
                json.Next = UrlHelper.GetPathAndQuery(json.Next);
            }

            return Ok(json);
        }
    }
}
