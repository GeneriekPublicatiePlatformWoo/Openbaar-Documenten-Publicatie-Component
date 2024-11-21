using Microsoft.AspNetCore.Mvc;
using ODPC.Apis.Odrc;

namespace ODPC.Features.Documenten.DocumentBijwerken
{
    [ApiController]
    public class DocumentBijwerkenController(IOdrcClientFactory clientFactory) : ControllerBase
    {
        [HttpPut("api/{apiVersion}/documenten/{uuid:guid}")]
        public async Task<IActionResult> Put(string apiVersion, Guid uuid, PublicatieDocument document, CancellationToken token)
        {
            using var client = clientFactory.Create("Document bijwerken");

            // TODO: check eigenaar

            var url = "/api/" + apiVersion + "/documenten/" + uuid;

            var response = await client.PutAsJsonAsync(url, document, token);

            response.EnsureSuccessStatusCode();

            var viewModel = await response.Content.ReadFromJsonAsync<PublicatieDocument>(token);

            return Ok(viewModel);
        }
    }
}
