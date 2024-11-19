using Microsoft.AspNetCore.Mvc;
using ODPC.Apis.Odrc;

namespace ODPC.Features.Publicaties.PublicatieDetails
{
    [ApiController]
    public class PublicatieDetailsController(IOdrcClientFactory clientFactory) : ControllerBase
    {
        [HttpGet("api/{apiVersion}/publicaties/{uuid:guid}")]
        public async Task<IActionResult> Put(string apiVersion, Guid uuid, CancellationToken token)
        {
            using var client = clientFactory.Create("Publicatie ophalen");
            var url = "/api/" + apiVersion + "/publicaties/" + uuid;

            var json = await client.GetFromJsonAsync<Publicatie>(url, token);

            return Ok(json);
        }
    }
}
