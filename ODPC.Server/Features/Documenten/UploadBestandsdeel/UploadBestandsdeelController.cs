using Microsoft.AspNetCore.Mvc;
using ODPC.Apis.Odrc;

namespace ODPC.Features.Documenten.UploadBestandsdeel
{
    [ApiController]
    [DisableRequestSizeLimit]
    public class UploadBestandsdeelController(IOdrcClientFactory clientFactory) : ControllerBase
    {
        [HttpPut("api/v1/documenten/bestandsdelen")]
        public async Task<IActionResult> Put(CancellationToken token)
        {
            var form = await Request.ReadFormAsync(token);

            if (form.Files.Count == 0)
            {
                return BadRequest("No file uploaded");
            }

            var endpoint = form["url"].ToString();

            if (string.IsNullOrEmpty(endpoint))
            {
                return BadRequest("No upload url");
            }
            else
            {
                endpoint = UrlHelper.GetPathAndQuery(endpoint);
            }

            var file = form.Files[0];
            var content = new MultipartFormDataContent();
            var fileContent = new StreamContent(file.OpenReadStream());

            content.Add(fileContent, "inhoud", file.FileName);

            using var client = clientFactory.Create("Upload bestandsdeel");
            var response = await client.PutAsync(endpoint, content, token);

            response.EnsureSuccessStatusCode();

            return NoContent();
        }
    }
}
