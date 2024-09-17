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
        /// Updates (vooralsnog) alleen de waardelijsten die gebruikt mogen worden binnen een groep 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>

        [HttpPut("api/gebruikersgroepen/{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] GebruikersgroepPutModel model, CancellationToken token)
        {
            var groep = await _context.Gebruikersgroepen.SingleOrDefaultAsync(x => x.Id == id, cancellationToken: token);

            if (groep == null) return NotFound();

            //verwijder bestaande waardelijsten voor deze groep
            await _context
                .GebruikersgroepWaardelijsten
                .Where(x=> x.GebruikersgroepId == id)
                .ExecuteDeleteAsync(cancellationToken: token);

            //voeg de nieuwe set waardelijsten toe aan deze groep
            _context.GebruikersgroepWaardelijsten
                .AddRange(model.GekoppeldeWaardelijsten.Select(x => new GebruikersgroepWaardelijst { GebruikersgroepId = id, WaardelijstId = x }));

            await _context.SaveChangesAsync(token);

            var result = new GebruikersgroepDetailsModel
            {
                Id = groep.Id,
                Name = groep.Name,
                GekoppeldeWaardelijsten = model.GekoppeldeWaardelijsten
            };

            return Ok(result);
        }
    }
}
