using Microsoft.AspNetCore.Mvc;
using ODPC.Apis.Odrc;
using ODPC.Authentication;

namespace ODPC.Features.Documenten.DocumentBijwerken
{
    [ApiController]
    public class DocumentBijwerkenController(IOdrcClientFactory clientFactory, OdpcUser user, ILogger<DocumentBijwerkenController> logger) : ControllerBase
    {
        [HttpPut("api/{version}/documenten/{uuid:guid}")]
        public async Task<IActionResult> Put(string version, Guid uuid, PublicatieDocument document, CancellationToken token)
        {
            using var client = clientFactory.Create("Document bijwerken");

            var url = $"/api/{version}/documenten/{uuid}";

            // document ophalen
            using var getResponse = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead, token);

            if (!getResponse.IsSuccessStatusCode)
            {
                return StatusCode(502);
            }

            var json = await getResponse.Content.ReadFromJsonAsync<PublicatieDocument>(token);

            if (json?.Eigenaar?.identifier != user.Id)
            {
                return NotFound();
            }

            // document bijwerken
            using var content = JsonContent.Create(document);
            await content.LoadIntoBufferAsync();
            using var putResponse = await client.PutAsync(url, content, token);
            if (!putResponse.IsSuccessStatusCode)
            {
                var error = await putResponse.Content.ReadAsStringAsync();
                logger.LogError("error in response: {body}" + error);
            }
            putResponse.EnsureSuccessStatusCode();

            var viewModel = await putResponse.Content.ReadFromJsonAsync<PublicatieDocument>(token);

            return Ok(viewModel);
        }
    }
}
