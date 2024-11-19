using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Mvc;
using ODPC.Apis.Odrc;

namespace ODPC.Features.Documenten.DocumentenOverzicht
{
    [ApiController]
    public class DocumentenOverzichtController(IOdrcClientFactory clientFactory) : ControllerBase
    {
        [HttpGet("api/{apiVersion}/documenten")]
        public async Task<IActionResult> Get(string apiVersion, [FromQuery] string publicatie, string? page, CancellationToken token)
        {
            // documenten ophalen uit het ODRC
            using var client = clientFactory.Create("Documenten ophalen");
            var url = "/api/" + apiVersion + "/documenten?publicatie=" + publicatie + "&page=" + page;

            var json = await client.GetFromJsonAsync<PagedResponseModel<JsonNode>>(url, token);

            return Ok(json);
        }
    }
}
