using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Mvc;
using ODPC.Apis.Odrc;

namespace ODPC.Features.Informatiecategorieen.AlleInformatiecategorieen
{
    [ApiController]
    public class InformatiecategorieenController(IOdrcClientFactory clientFactory) : ControllerBase
    {
        [HttpGet("api/{apiVersion}/informatiecategorieen")]
        public async Task<IActionResult> Get(string apiVersion, CancellationToken token, [FromQuery] string? page = "1")
        {
            // infocategorien ophalen uit het ODRC
            using var client = clientFactory.Create("Informatiecategorieen ophalen");
            var url = "/api/" + apiVersion + "/informatiecategorieen?page=" + page;

            var json = await client.GetFromJsonAsync<PagedResponseModel<JsonNode>>(url, token);

            return Ok(json);
        }
    }
}
