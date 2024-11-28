using Microsoft.AspNetCore.Mvc;
using ODPC.Apis.Odrc;
using ODPC.Authentication;

namespace ODPC.Features.Documenten.DocumentDownload
{
    public class DocumentDownloadController(IOdrcClientFactory clientFactory, OdpcUser user) : ControllerBase
    {
        [HttpGet("api/{version}/documenten/{uuid:guid}/download")]
        public async Task<IActionResult> Get(string version, Guid uuid, CancellationToken token)
        {
            using var client = clientFactory.Create("Document ophalen");

            var url = $"/api/{version}/documenten/{uuid}";

            using var response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead, token);

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode(502);
            }

            var json = await response.Content.ReadFromJsonAsync<PublicatieDocument>(token);

            return json?.Eigenaar?.identifier != user.Id ? NotFound() : new DocumentDownloadResult(Request.Path, "Document downloaden");
        }
    }
}
