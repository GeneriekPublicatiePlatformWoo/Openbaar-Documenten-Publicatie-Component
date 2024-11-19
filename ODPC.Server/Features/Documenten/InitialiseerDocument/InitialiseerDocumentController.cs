using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ODPC.Apis.Odrc;
using ODPC.Features.Publicaties;

namespace ODPC.Features.Documenten.InitialiseerDocument
{
    [ApiController]
    public class InitialiseerDocumentController(IOdrcClientFactory clientFactory) : ControllerBase
    {
        [HttpPost("api/{apiVersion}/documenten")]
        public async Task<IActionResult> Post(string apiVersion, PublicatieDocument document, CancellationToken token)
        {
            using var client = clientFactory.Create("Initialiseer document");
            var url = "/api/" + apiVersion + "/documenten";

            var response = await client.PostAsJsonAsync(url, document, token);

            response.EnsureSuccessStatusCode();

            var viewModel = await response.Content.ReadFromJsonAsync<PublicatieDocument>(token);

            return Ok(viewModel);
        }
    }
}
