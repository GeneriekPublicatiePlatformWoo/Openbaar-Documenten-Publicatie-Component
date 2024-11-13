using Microsoft.AspNetCore.Mvc;
using ODPC.Apis.Odrc;

namespace ODPC.Features.Publicaties.PublicatieRegistreren
{
    [ApiController]
    public class PublicatieRegistrerenController(IOdrcClientFactory clientFactory, IGebruikerWaardelijstItemsService waardelijstItemsService) : ControllerBase
    {
        private readonly IOdrcClientFactory _clientFactory = clientFactory;

        [HttpPost("api/v1/publicaties")]
        public async Task<IActionResult> Post(Publicatie publicatie, CancellationToken token)
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

            var client = _clientFactory.Create("Publicatie geregistreerd");
            var response = await client.PostAsJsonAsync("/api/v1/publicaties", publicatie, token);

            response.EnsureSuccessStatusCode();
            var viewModel = await response.Content.ReadFromJsonAsync<Publicatie>(token);

            // TODO deze regel kan eraf als deze story is geimplementeerd: https://github.com/GeneriekPublicatiePlatformWoo/registratie-component/issues/49
            viewModel!.Status = publicatie.Status;

            // TODO de mock laten we er nog even in totdat ook het ophalen van gegevens via het register verloopt: https://github.com/GeneriekPublicatiePlatformWoo/Openbaar-Documenten-Publicatie-Component/issues/68
            PublicatiesMock.Publicaties[viewModel!.Uuid] = viewModel;

            return Ok(viewModel);
        }
    }
}
