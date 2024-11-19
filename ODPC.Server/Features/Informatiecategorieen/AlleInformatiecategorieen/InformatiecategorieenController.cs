using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Mvc;
using ODPC.Apis.Odrc;

namespace ODPC.Features.Informatiecategorieen.AlleInformatiecategorieen
{
    [ApiController]
    public class InformatiecategorieenController(IOdrcClientFactory clientFactory) : ControllerBase
    {
        [HttpGet("api/{apiVersion}/informatiecategorieen")]
        public async Task<IActionResult> Get(string apiVersion, [FromQuery] string? page, CancellationToken token)
        {
            // infocategorien ophalen uit het ODRC
            using var client = clientFactory.Create("Informatiecategorieen ophalen");
            var url = "/api/" + apiVersion + "/informatiecategorieen?page=" + page;

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
