using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using ODPC.Apis.Odrc;
using ODPC.Authentication;
using ODPC.Config;

namespace ODPC.Features.Publicaties.PublicatieRegistreren
{
    [ApiController]
    public class PublicatieRegistrerenController(OdpUser user, IOrdcClientFactory clientFactory) : ControllerBase
    {
        private readonly IOrdcClientFactory _clientFactory = clientFactory;

        [HttpPost("api/v1/publicaties")]
        public async Task<IActionResult> Post(Publicatie publicatie)
        {
            publicatie.Uuid = Guid.NewGuid();
            publicatie.Registratiedatum = DateTime.Now;

            //de mock laten we er nog even in totdat ook het ophalen van gegevens via het register verloopt
            PublicatiesMock.Publicaties[publicatie.Uuid] = publicatie;

            var client = _clientFactory.Create(user, "Publicatie geregistreerd");

            var response = await client.PostAsJsonAsync("/api/v1/publicaties/", publicatie, new CancellationToken());
            
            response.EnsureSuccessStatusCode();
            
            var responseBody = await response.Content.ReadAsStringAsync();                   

            var viewModel = JsonSerializer.Deserialize<Publicatie>(responseBody, JsonSerialization.Options);

            return Ok(viewModel);
        }

    }
}
