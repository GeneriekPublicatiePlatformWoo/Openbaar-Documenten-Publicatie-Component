using Microsoft.AspNetCore.Mvc;

namespace ODPC.Features.Publicaties.PublicatieBijwerken
{
    [ApiController]
    public class PublicatieBijwerkenController(IGebruikerWaardelijstItemsService waardelijstItemsService) : ControllerBase
    {
        [HttpPut("api/v1/publicaties/{uuid}")]
        public async Task<IActionResult> Put(Guid uuid, Publicatie publicatie, CancellationToken token)
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

            PublicatiesMock.Publicaties[uuid] = publicatie;
            return Ok(publicatie);
        }
    }
}
