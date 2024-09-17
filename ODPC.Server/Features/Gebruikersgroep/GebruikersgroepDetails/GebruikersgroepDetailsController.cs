using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ODPC.Data;

namespace ODPC.Features.Gebruikersgroep.GebruikersgroepDetails
{

    [ApiController]
    public class GebruikersgroepDetailsController(OdpcDbContext context) : ControllerBase
    {
        private readonly OdpcDbContext _context = context;

        [HttpGet("api/gebruikersgroepen/{Id}")]
        public async Task<IActionResult> Get(Guid id, CancellationToken token)
        {
            var groep = await _context.Gebruikersgroepen.SingleOrDefaultAsync(x => x.Id == id, cancellationToken: token);

            if (groep == null) return NotFound();

            var result = new GebruikersgroepDetailsModel
            {
                Id = groep.Id,
                Name = groep.Name,
                GekoppeldeWaardelijsten = await _context
                    .GebruikersgroepWaardelijsten
                    .Where(x => x.GebruikersgroepId == id)
                    .Select(x => x.WaardelijstId)
                    .ToListAsync(cancellationToken: token)
            };

            return Ok(result);
        }
    }
}
