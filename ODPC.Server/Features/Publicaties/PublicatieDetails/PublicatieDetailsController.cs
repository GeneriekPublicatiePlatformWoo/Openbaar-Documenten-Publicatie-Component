using Microsoft.AspNetCore.Mvc;
using ODPC.Apis.Odrc;

namespace ODPC.Features.Publicaties.PublicatieDetails
{
    [ApiController]
    public class PublicatieDetailsController(IOdrcClientFactory clientFactory) : ControllerBase
    {
        [HttpGet("api/v1/publicaties/{uuid:guid}")]
        public async Task<IActionResult> Put(Guid uuid, CancellationToken token)
        {
            using var client = clientFactory.Create("Publicatie ophalen");
            var response = await client.GetAsync("/api/v1/publicaties/" + uuid, token);

            response.EnsureSuccessStatusCode();

            var viewModel = await response.Content.ReadFromJsonAsync<Publicatie>(token);

            return Ok(viewModel);
        }
    }
}
