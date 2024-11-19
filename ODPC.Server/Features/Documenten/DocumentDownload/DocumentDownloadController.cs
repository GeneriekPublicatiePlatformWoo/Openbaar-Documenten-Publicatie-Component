using System.Threading;
using Microsoft.AspNetCore.Mvc;
using ODPC.Apis.Odrc;

namespace ODPC.Features.Documenten.DocumentDownload
{
    public class DocumentDownloadController(IOdrcClientFactory clientFactory) : ControllerBase
    {
        [HttpGet("api/{apiVersion}/documenten/{uuid:guid}/download")]
        public async Task<IActionResult> Get(string apiVersion, Guid uuid, CancellationToken token)
        {
            using var client = clientFactory.Create("Document downloaden");
            var url = "/api/" + apiVersion + "/documenten/" + uuid + "/download";

            var response = await client.GetAsync(url, token);

            response.EnsureSuccessStatusCode();

            var contentType = response.Content.Headers.ContentType?.ToString() ?? "application/octet-stream";
            var fileName = response.Content.Headers.ContentDisposition?.FileName?.Trim('"') ?? "woo_document";
            var fileStream = await response.Content.ReadAsStreamAsync(token);

            return File(fileStream, contentType, fileName);
        }
    }
}
