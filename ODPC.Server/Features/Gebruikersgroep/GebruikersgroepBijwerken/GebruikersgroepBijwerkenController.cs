using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ODPC.Data;
using ODPC.Data.Entities;

namespace ODPC.Features.Gebruikersgroep.GebruikersgroepBijwerken
{
    [ApiController]
    public class GebruikersgroepBijwerkenController(OdpcDbContext context) : ControllerBase
    {

        private readonly OdpcDbContext _context = context;

        /// <summary>
        /// Voegt een groep toe, inclusief de waardelijsten die gebruikt mogen worden binnen die groep en de gebruikers die binnen die groep vallen
        /// </summary>
        /// <param name="model"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost("api/gebruikersgroepen")]
        public async Task<IActionResult> Post([FromBody] GebruikersgroepUpsertModel model, CancellationToken token)
        {
            var groep = new Data.Entities.Gebruikersgroep { Naam = model.Naam, Omschrijving = model.Omschrijving };
            return await UpdateGroep(model, groep, token);
        }

        /// <summary>
        /// Update de waardelijsten die gebruikt mogen worden binnen een groep en de gebruikers die binnen die groep vallen
        /// </summary>
        /// <param name="uuid"></param>
        /// <param name="model"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPut("api/gebruikersgroepen/{uuid:guid}")]
        public async Task<IActionResult> Put(Guid uuid, [FromBody] GebruikersgroepUpsertModel model, CancellationToken token)
        {
            var groep = await _context.Gebruikersgroepen.SingleOrDefaultAsync(x => x.Uuid == uuid, cancellationToken: token);
            if (groep == null) return NotFound();
            return await UpdateGroep(model, groep, token);
        }

        private async Task<IActionResult> UpdateGroep(GebruikersgroepUpsertModel model, Data.Entities.Gebruikersgroep groep, CancellationToken token)
        {
            groep.Naam = model.Naam;
            groep.Omschrijving = model.Omschrijving;

            //verwijder bestaande waardelijsten voor deze groep
            await _context
                .GebruikersgroepWaardelijsten
                .Where(x => x.GebruikersgroepUuid == groep.Uuid)
                .ExecuteDeleteAsync(cancellationToken: token);

            //voeg de nieuwe set waardelijsten toe aan deze groep
            _context.GebruikersgroepWaardelijsten
                .AddRange(model.GekoppeldeWaardelijsten.Select(x => new GebruikersgroepWaardelijst { Gebruikersgroep = groep, WaardelijstId = x }));

            //verwijder bestaande gebruikers voor deze groep
            await _context
                .GebruikersgroepGebruikers
                .Where(x => x.GebruikersgroepUuid == groep.Uuid)
                .ExecuteDeleteAsync(cancellationToken: token);

            //voeg de nieuwe set gebruikers toe aan deze groep
            _context.GebruikersgroepGebruikers
                .AddRange(model.GekoppeldeGebruikers.Select(x => new GebruikersgroepGebruiker { Gebruikersgroep = groep, GebruikerId = x }));

            await _context.SaveChangesAsync(token);

            var result = new GebruikersgroepDetailsModel
            {
                Uuid = groep.Uuid,
                Naam = groep.Naam,
                Omschrijving = groep.Omschrijving,
                GekoppeldeWaardelijsten = model.GekoppeldeWaardelijsten,
                GekoppeldeGebruikers = model.GekoppeldeGebruikers,
            };

            return Ok(result);
        }
    }
}
