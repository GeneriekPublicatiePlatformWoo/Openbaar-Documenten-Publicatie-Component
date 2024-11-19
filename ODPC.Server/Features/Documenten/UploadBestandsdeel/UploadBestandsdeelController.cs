﻿using Microsoft.AspNetCore.Mvc;
using ODPC.Apis.Odrc;

namespace ODPC.Features.Documenten.UploadBestandsdeel
{
    [ApiController]
    [DisableRequestSizeLimit]
    public class UploadBestandsdeelController(IOdrcClientFactory clientFactory) : ControllerBase
    {
        [HttpPut("api/{apiVersion}/documenten/{docUuid:guid}/bestandsdelen/{partUuid:guid}")]
        public async Task<IActionResult> Put(string apiVersion, Guid docUuid, Guid partUuid, CancellationToken token)
        {
            var form = await Request.ReadFormAsync(token);

            if (form.Files.Count == 0)
            {
                return BadRequest("No file uploaded");
            }

            var file = form.Files[0];
            var content = new MultipartFormDataContent();
            var fileContent = new StreamContent(file.OpenReadStream());

            content.Add(fileContent, "inhoud", file.FileName);

            using var client = clientFactory.Create("Upload bestandsdeel");
            var url = "/api/" + apiVersion + "/documenten/" + docUuid + "/bestandsdelen/" + partUuid;

            var response = await client.PutAsync(url, content, token);

            response.EnsureSuccessStatusCode();

            return NoContent();
        }
    }
}
