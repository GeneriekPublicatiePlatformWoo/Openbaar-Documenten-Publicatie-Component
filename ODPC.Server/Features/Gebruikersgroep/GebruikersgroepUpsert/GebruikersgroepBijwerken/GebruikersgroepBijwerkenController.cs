using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ODPC.Data;
using ODPC.Data.Entities;

namespace ODPC.Features.Gebruikersgroep.GebruikersgroepUpsert
{
    [ApiController]
    public class GebruikersgroepBijwerkenController(OdpcDbContext context) : ControllerBase
    {

        private readonly OdpcDbContext _context = context;


        /// <summary>
        /// gebruikersgroep aanmaken
        /// </summary>
        /// <param name="uuid"></param>
        /// <param name="model"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPut("api/v1/gebruikersgroepen/{uuid:guid}")]
        public async Task<IActionResult> Put(Guid uuid, [FromBody] GebruikersgroepUpsertModel model, CancellationToken token)
        {
            var groep = await _context.Gebruikersgroepen.SingleOrDefaultAsync(x => x.Uuid == uuid, cancellationToken: token);
            if (groep == null) return NotFound();

            //update groep waardes
            groep.Naam = model.Naam;
            groep.Omschrijving = model.Omschrijving;

            //verwijder bestaande waardelijsten voor deze groep                   
            await _context
               .GebruikersgroepWaardelijsten
               .Where(x => x.GebruikersgroepUuid == groep.Uuid)
               .ExecuteDeleteAsync(cancellationToken: token);                     

            //voeg de nieuwe selectie waardelijsten toe aan deze groep
            UpsertHelpers.AddWaardelijstenToGroep(model.GekoppeldeWaardelijsten, groep, _context);

            await _context.SaveChangesAsync(token);

            return base.Ok(GebruikersgroepDetailsModel.MapEntityToViewModel(groep));
        }



    
    }
}
