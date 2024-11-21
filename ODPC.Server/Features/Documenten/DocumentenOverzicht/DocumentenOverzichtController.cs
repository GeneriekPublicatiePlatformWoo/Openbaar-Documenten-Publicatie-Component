using System;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Mvc;
using ODPC.Apis.Odrc;
using ODPC.Authentication;
using ODPC.Features.Publicaties;

namespace ODPC.Features.Documenten.DocumentenOverzicht
{
    [ApiController]
    public class DocumentenOverzichtController(IOdrcClientFactory clientFactory) : ControllerBase
    {
        [HttpGet("api/{apiVersion}/documenten")]
        public async Task<IActionResult> Get(
            string apiVersion,
            [FromQuery] string publicatie,
            OdpcUser user,
            CancellationToken token,
            [FromQuery] string? page = "1")
        {
            // documenten ophalen uit het ODRC
            using var client = clientFactory.Create("Documenten ophalen");

            var noResult = new PagedResponseModel<JsonNode>
            {
                Results = new List<JsonNode>(),
                Count = 0,
                Next = null,
                Previous = null
            };

            // TODO: hiervoor komt filter op eigenaar in ODRC
            var publicatieUrl = "/api/" + apiVersion + "/publicaties/" + publicatie;
            var publicatieJson = await client.GetFromJsonAsync<Publicatie>(publicatieUrl, token);

            if (publicatieJson == null || publicatieJson.Eigenaar?.identifier != user.Id)
            {
                return Ok(noResult);
            }

            var documentenUrl = "/api/" + apiVersion + "/documenten?publicatie=" + publicatie + "&page=" + page;
            var documentenJson = await client.GetFromJsonAsync<PagedResponseModel<JsonNode>>(documentenUrl, token);

            return documentenJson != null ? Ok(documentenJson) : Ok(noResult);
        }
    }
}
