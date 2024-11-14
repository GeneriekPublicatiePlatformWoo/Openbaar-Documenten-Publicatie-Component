using System.Threading;
using Microsoft.AspNetCore.Mvc;
using ODPC.Apis.Odrc;

namespace ODPC.Features.Documenten.DocumentDownload
{
    public class DocumentDownloadController(IOdrcClientFactory clientFactory) : ControllerBase
    {
        private readonly IOdrcClientFactory _clientFactory = clientFactory;

        [HttpGet("api/v1/documenten/{uuid:guid}/download")]
        public async Task<IActionResult> Get(Guid uuid, CancellationToken token)
        {
            var client = _clientFactory.Create("Document downloaden");
            var response = await client.GetAsync("/api/v1/documenten/" + uuid + "/download", token);

            response.EnsureSuccessStatusCode();

            var contentType = response.Content.Headers.ContentType?.ToString() ?? "application/octet-stream";
            // response.Content.Headers.ContentDisposition?.FileName
            var fileName = "document";
            var fileStream = await response.Content.ReadAsStreamAsync(token);

            return File(fileStream, contentType, fileName);
        }
    }
}
