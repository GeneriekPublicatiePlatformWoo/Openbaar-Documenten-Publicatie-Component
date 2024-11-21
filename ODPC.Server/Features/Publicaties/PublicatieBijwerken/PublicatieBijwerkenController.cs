using Microsoft.AspNetCore.Mvc;
using ODPC.Apis.Odrc;
using ODPC.Authentication;

namespace ODPC.Features.Publicaties.PublicatieBijwerken
{
    [ApiController]
    public class PublicatieBijwerkenController(
        IOdrcClientFactory clientFactory,
        IGebruikerWaardelijstItemsService waardelijstItemsService,
        OdpcUser user) : ControllerBase
    {
        [HttpPut("api/{apiVersion}/publicaties/{uuid:guid}")]
        public async Task<IActionResult> Put(string apiVersion, Guid uuid, Publicatie publicatie, CancellationToken token)
        {
            var waardelijstItems = await waardelijstItemsService.GetAsync(token);

            if (publicatie.Publisher != null && !waardelijstItems.Contains(publicatie.Publisher))
            {
                ModelState.AddModelError(nameof(publicatie.Publisher), "Gebruiker is niet geautoriseerd voor deze organisatie");
                return BadRequest(ModelState);
            }

            if (publicatie.InformatieCategorieen != null && publicatie.InformatieCategorieen.Any(c => !waardelijstItems.Contains(c)))
            {
                ModelState.AddModelError(nameof(publicatie.InformatieCategorieen), "Gebruiker is niet geautoriseerd voor deze informatiecategorieën");
                return BadRequest(ModelState);
            }

            using var client = clientFactory.Create("Publicatie bijwerken");

            // publicatie ophalen
            var url = "/api/" + apiVersion + "/publicaties/" + uuid;

            var publicatieJson = await client.GetFromJsonAsync<Publicatie>(url, token);

            if (publicatieJson == null || publicatieJson.Eigenaar?.identifier != user.Id)
            {
                return NotFound();
            }

            // publicatie bijwerken
            var response = await client.PutAsJsonAsync(url, publicatie, token);

            response.EnsureSuccessStatusCode();

            var viewModel = await response.Content.ReadFromJsonAsync<Publicatie>(token);

            return Ok(viewModel);
        }
    }
}
