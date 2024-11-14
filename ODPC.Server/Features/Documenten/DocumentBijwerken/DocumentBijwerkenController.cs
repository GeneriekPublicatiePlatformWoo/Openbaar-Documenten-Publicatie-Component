using Microsoft.AspNetCore.Mvc;
using ODPC.Apis.Odrc;

namespace ODPC.Features.Documenten.DocumentBijwerken
{
    [ApiController]
    public class DocumentBijwerkenController(IOdrcClientFactory clientFactory) : ControllerBase
    {
        private readonly IOdrcClientFactory _clientFactory = clientFactory;

        [HttpPut("api/v1/documenten/{uuid:guid}")]
        public async Task<IActionResult> Put(Guid uuid, PublicatieDocument document, CancellationToken token)
        {
            var client = _clientFactory.Create("Document bijwerken");
            var response = await client.PutAsJsonAsync("/api/v1/documenten/" + uuid, document, token);

            response.EnsureSuccessStatusCode();

            var viewModel = await response.Content.ReadFromJsonAsync<PublicatieDocument>(token);

            return Ok(viewModel);
        }
    }
}
