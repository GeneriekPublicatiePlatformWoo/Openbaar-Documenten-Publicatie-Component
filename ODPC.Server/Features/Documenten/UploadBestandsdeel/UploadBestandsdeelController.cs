using Microsoft.AspNetCore.Mvc;
using ODPC.Apis.Odrc;

namespace ODPC.Features.Documenten.UploadBestandsdeel
{
    [ApiController]
    [DisableRequestSizeLimit]
    public class UploadBestandsdeelController(IOdrcClientFactory clientFactory) : ControllerBase
    {
        [HttpPut("api/v1/bestandsdelen")]
        public async Task<IActionResult> Put(CancellationToken token)
        {
            var form = await Request.ReadFormAsync(token);

            if (form.Files.Count == 0)
            {
                return BadRequest("No file uploaded");
            }

            var url = form["url"].ToString();

            if (string.IsNullOrEmpty(url))
            {
                return BadRequest("No url");
            }
            else
            {
                url = UrlHelper.GetPathAndQuery(url);
            }

            var file = form.Files[0];
            var content = new MultipartFormDataContent();
            var fileContent = new StreamContent(file.OpenReadStream());

            content.Add(fileContent, "inhoud", file.FileName);

            using var client = clientFactory.Create("Upload bestandsdeel");
            var response = await client.PutAsync(url, content, token);

            response.EnsureSuccessStatusCode();

            return NoContent();
        }
    }
}
