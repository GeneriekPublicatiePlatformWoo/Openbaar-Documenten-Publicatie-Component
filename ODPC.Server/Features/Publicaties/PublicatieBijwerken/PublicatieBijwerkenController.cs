using Microsoft.AspNetCore.Mvc;

namespace ODPC.Features.Publicaties.PublicatieBijwerken
{
    [ApiController]
    public class PublicatieBijwerkenController(IGebruikerWaardelijstItemsService waardelijstItemsService) : ControllerBase
    {
        [HttpPut("api/v1/publicaties/{uuid}")]
        public async Task<IActionResult> Put(Guid uuid, Publicatie publicatie, CancellationToken token)
        {
            var categorieen = await waardelijstItemsService.GetAsync(token);

            if (publicatie.GekoppeldeInformatiecategorieen != null && publicatie.GekoppeldeInformatiecategorieen.Any(c => !categorieen.Contains(c)))
            {
                ModelState.AddModelError(nameof(publicatie.GekoppeldeInformatiecategorieen), "Gebruiker is niet geautoriseerd voor deze informatiecategorieën");
                return BadRequest(ModelState);
            }

            PublicatiesMock.Publicaties[uuid] = publicatie;
            return Ok(publicatie);
        }
    }
}
