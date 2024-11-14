using Microsoft.AspNetCore.Mvc;
using ODPC.Apis.Odrc;

namespace ODPC.Features.Documenten.InitialiseerDocument
{
    [ApiController]
    public class InitialiseerDocumentController(IOdrcClientFactory clientFactory) : ControllerBase
    {
        private readonly IOdrcClientFactory _clientFactory = clientFactory;

        [HttpPost("api/v1/documenten")]
        public async Task<IActionResult> Post(PublicatieDocument document, CancellationToken token)
        {
            var client = _clientFactory.Create("Initialiseer document");
            var response = await client.PostAsJsonAsync("/api/v1/documenten", document, token);

            response.EnsureSuccessStatusCode();

            var viewModel = await response.Content.ReadFromJsonAsync<PublicatieDocument>(token);

            return Ok(viewModel);
        }
    }
}
