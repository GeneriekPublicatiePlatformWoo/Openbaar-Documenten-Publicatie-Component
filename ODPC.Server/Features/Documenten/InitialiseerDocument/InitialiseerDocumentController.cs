using Microsoft.AspNetCore.Mvc;
using ODPC.Apis.Odrc;

namespace ODPC.Features.Documenten.InitialiseerDocument
{
    [ApiController]
    public class InitialiseerDocumentController(IOdrcClientFactory clientFactory, ILogger<InitialiseerDocumentController> logger) : ControllerBase
    {
        [HttpPost("api/{version}/documenten")]
        public async Task<IActionResult> Post(string version, PublicatieDocument document, CancellationToken token)
        {
            using var client = clientFactory.Create("Initialiseer document");

            var url = $"/api/{version}/documenten";

            using var content = JsonContent.Create(document);
            await content.LoadIntoBufferAsync();
            using var response = await client.PostAsync(url, content, token);
            if (!response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                logger.LogError("error in response: {body}" + json);
            }
            response.EnsureSuccessStatusCode();

            var viewModel = await response.Content.ReadFromJsonAsync<PublicatieDocument>(token);

            return Ok(viewModel);
        }
    }
}
