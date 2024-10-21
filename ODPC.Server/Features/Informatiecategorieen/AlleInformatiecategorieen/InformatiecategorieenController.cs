using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Mvc;
using ODPC.Apis.Odrc;
using ODPC.Authentication;

namespace ODPC.Features.Informatiecategorieen.AlleInformatiecategorieen
{
    [ApiController]
    public class InformatiecategorieenController(OdpcUser user, IOdrcClientFactory clientFactory) : ControllerBase
    {
        [HttpGet("api/v1/informatiecategorieen")]
        public async Task<IActionResult> Get([FromQuery] string? page, CancellationToken token)
        {
            //infocategorien ophalen uit het ODRC
            //user en audit log handeling meegeven is misschien nog niet nodig, maar doet geen kwaad
            using var client = clientFactory.Create(user, "Alle informatiecategorieen ophalen");
            var json = await client.GetFromJsonAsync<JsonNode>("/api/v1/informatiecategorieen?page=" + page, token);
            return Ok(json);
        }
    }
}
