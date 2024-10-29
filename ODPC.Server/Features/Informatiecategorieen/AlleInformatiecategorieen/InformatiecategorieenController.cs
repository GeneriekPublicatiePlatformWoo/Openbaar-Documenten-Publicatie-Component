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
            var json = await client.GetFromJsonAsync<PagedResponseModel<JsonNode>>("/api/v1/informatiecategorieen?page=" + page, token);
            if (json != null)
            {
                json.Previous = GetPathAndQuery(json.Previous);
                json.Next = GetPathAndQuery(json.Next);
            }
            return Ok(json);
        }

        private static string? GetPathAndQuery(string? url) => Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out var uri) 
            ? uri.PathAndQuery 
            : url;
    }
}
