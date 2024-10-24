using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Mvc;
using ODPC.Apis.Odrc;

namespace ODPC.Features.Informatiecategorieen.AlleInformatiecategorieen
{
    [ApiController]
    public class InformatiecategorieenController(IOdrcClientFactory clientFactory) : ControllerBase
    {
        [HttpGet("api/v1/informatiecategorieen")]
        public async Task<IActionResult> Get([FromQuery] string? page, CancellationToken token)
        {
            //infocategorien ophalen uit het ODRC
            using var client = clientFactory.Create("Alle informatiecategorieen ophalen");
            var json = await client.GetFromJsonAsync<JsonNode>("/api/v1/informatiecategorieen?page=" + page, token);
            return Ok(json);
        }
    }
}
