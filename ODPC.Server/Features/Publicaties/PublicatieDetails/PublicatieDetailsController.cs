using Microsoft.AspNetCore.Mvc;
using ODPC.Apis.Odrc;
using ODPC.Authentication;

namespace ODPC.Features.Publicaties.PublicatieDetails
{
    [ApiController]
    public class PublicatieDetailsController(IOdrcClientFactory clientFactory, OdpcUser user) : ControllerBase
    {
        [HttpGet("api/{version}/publicaties/{uuid:guid}")]
        public async Task<IActionResult> Put(string version, Guid uuid, CancellationToken token)
        {
            using var client = clientFactory.Create("Publicatie ophalen");

            var url = $"/api/{version}/publicaties/{uuid}";

            using var response = await client.GetAsync(url, HttpCompletionOption.ResponseContentRead, token);

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode(502);
            }

            var json = await response.Content.ReadFromJsonAsync<Publicatie>(token);

            return json?.Eigenaar?.identifier == user.Id ? Ok(json) : NotFound();
        }
    }
}
