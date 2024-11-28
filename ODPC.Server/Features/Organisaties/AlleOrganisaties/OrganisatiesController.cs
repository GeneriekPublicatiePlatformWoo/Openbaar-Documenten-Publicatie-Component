using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Mvc;
using ODPC.Apis.Odrc;

namespace ODPC.Features.Organisaties.AlleOrganisaties
{
    [ApiController]
    public class OrganisatiesController(IOdrcClientFactory clientFactory) : ControllerBase
    {
        [HttpGet("api/{version}/organisaties")]
        public async Task<IActionResult> Get(string version, CancellationToken token, [FromQuery] string? page = "1")
        {
            // organisaties ophalen uit het ODRC
            using var client = clientFactory.Create("Organisaties ophalen");
            var url = $"/api/{version}/organisaties?page={page}";

            using var response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead, token);

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode(502);
            }

            var json = await response.Content.ReadFromJsonAsync<PagedResponseModel<JsonNode>>(token);

            return Ok(json);
        }
    }
}
