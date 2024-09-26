using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ODPC.Data;

namespace ODPC.Features.Gebruikersgroep.GebruikersgroepDetails
{

    [ApiController]
    public class GebruikersgroepDetailsController(OdpcDbContext context) : ControllerBase
    {
        private readonly OdpcDbContext _context = context;

        [HttpGet("api/gebruikersgroepen/{uuid:guid}")]
        public async Task<IActionResult> Get(Guid uuid, CancellationToken token)
        {
            var groep = await _context.Gebruikersgroepen.SingleOrDefaultAsync(x => x.Uuid == uuid, cancellationToken: token);

            if (groep == null) return NotFound();

            var result = new GebruikersgroepDetailsModel
            {
                Uuid = groep.Uuid,
                Naam = groep.Naam,
                Omschrijving = groep.Omschrijving,
                GekoppeldeWaardelijsten = await _context
                    .GebruikersgroepWaardelijsten
                    .Where(x => x.GebruikersgroepUuid == uuid)
                    .Select(x => x.WaardelijstId)
                    .ToListAsync(cancellationToken: token),
                GekoppeldeGebruikers = await _context
                    .GebruikersgroepGebruikers
                    .Where(x => x.GebruikersgroepUuid == uuid)
                    .Select(x => x.GebruikerId)
                    .ToListAsync(cancellationToken: token),
            };

            return Ok(result);
        }
    }
}
