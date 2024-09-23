using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ODPC.Authentication;
using ODPC.Data;
using ODPC.Data.Entities;

namespace ODPC.Features.Gebruikersgroep.GebruikersgroepBijwerken
{

    [ApiController]
    public class GebruikersgroepBijwerkenController(OdpcDbContext context, OdpcUser user) : ControllerBase
    {
        /// <summary>
        /// Updates (vooralsnog) alleen de waardelijsten die gebruikt mogen worden binnen een groep 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>

        [HttpPut("api/gebruikersgroepen/{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] GebruikersgroepPutModel model, CancellationToken token)
        {
            var auditregel = new Auditregel
            {
                GebruikersId = user.Id,
                Actie = AuditregelActie.update,
                ActieWeergave = "Gebruikersgroep aanpassen",
                Geslaagd = true,
                Resultaat = System.Net.HttpStatusCode.OK,
                ResultaatWeergave = "Gebruikersgroep aangepast",
                ResourceUuid = id,
                Resource = "Gebruikersgroep",
                Wijzigingen = new AuditregelWijziging(),
            };

            await context.AddAsync(auditregel, token);

            var groep = await context.Gebruikersgroepen.SingleOrDefaultAsync(x => x.Id == id, cancellationToken: token);

            if (groep == null)
            {
                auditregel.Geslaagd = false;
                auditregel.ResultaatWeergave = "Gebruikersgroep niet gevonden";
                auditregel.Resultaat = System.Net.HttpStatusCode.NotFound;
                await context.SaveChangesAsync(token);
                return NotFound();
            }

            var oldWaardelijsten = await context
                .GebruikersgroepWaardelijsten
                .Where(x => x.GebruikersgroepId == id)
                .Select(x => x.WaardelijstId)
                .OrderBy(x => x)
                .ToListAsync(cancellationToken: token);

            //verwijder bestaande waardelijsten voor deze groep
            context.GebruikersgroepWaardelijsten.RemoveRange(oldWaardelijsten
                .Where(x => !model.GekoppeldeWaardelijsten.Contains(x))
                .Select(x => new GebruikersgroepWaardelijst { GebruikersgroepId = id, WaardelijstId = x }));

            //voeg de nieuwe set waardelijsten toe aan deze groep
            await context.GebruikersgroepWaardelijsten
                .AddRangeAsync(model.GekoppeldeWaardelijsten
                    .Where(x => !oldWaardelijsten.Contains(x))
                    .Select(x => new GebruikersgroepWaardelijst { GebruikersgroepId = id, WaardelijstId = x }), token);

            var result = new GebruikersgroepDetailsModel
            {
                Id = groep.Id,
                Name = groep.Name,
                GekoppeldeWaardelijsten = model.GekoppeldeWaardelijsten.OrderBy(x => x).ToList()
            };

            auditregel.ResourceWeergave = groep.Name;
            auditregel.Wijzigingen.Oud = new GebruikersgroepDetailsModel
            {
                Id = groep.Id,
                Name = groep.Name,
                GekoppeldeWaardelijsten = oldWaardelijsten,
            }.ToJsonNode();
            auditregel.Wijzigingen.Nieuw = result.ToJsonNode();

            await context.SaveChangesAsync(token);

            return Ok(result);
        }
    }
}
