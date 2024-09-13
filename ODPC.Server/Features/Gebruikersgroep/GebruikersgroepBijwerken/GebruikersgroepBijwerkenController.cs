using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ODPC.Data;
using ODPC.Data.Entities;
using ODPC.Features.Gebruikersgroep.GebruikersgroepDetails;

namespace ODPC.Features.Gebruikersgroep.GebruikersgroepBijwerken
{
    [ApiController]
    public class GebruikersgroepBijwerkenController(OdpcDbContext context, ILogger<GebruikersgroepDetailsController> logger) : ControllerBase
    {

        private readonly OdpcDbContext _context = context;
        private readonly ILogger<GebruikersgroepDetailsController> _logger = logger;

        /// <summary>
        /// Updates (vooralsnog) alleen de waardelijsten die gebruikt mogen worden binnen een groep 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>

        [HttpPut("api/gebruikersgroepen/{id}")]
        public GebruikersgroepDetailsModel Put(Guid id, [FromBody] GebruikersgroepPutModel model)
        {
            var groep = _context.Gebruikersgroepen
                .Single(x => x.Id == id);

            //verwijder bestaande waardelijsten voor deze groep
            var gebruikersGroepen = _context
                .GebruikersgroepWaardelijsten
                .Where(x => x.GebruikersgroepId == id)
                .ToList();

            _context.GebruikersgroepWaardelijsten
                .RemoveRange(gebruikersGroepen);

            //voeg de nieuwe set waardelijsten toe aan deze groep
            _context.GebruikersgroepWaardelijsten
                .AddRange(model.GekoppeldeWaardelijsten.Select(x => new GebruikersgroepWaardelijst { Gebruikersgroep = groep, WaardelijstId = x }));

            _context.SaveChanges();

            return new GebruikersgroepDetailsModel
            {
                Id = groep.Id,
                Name = groep.Name,
                GekoppeldeWaardelijsten = _context
                    .GebruikersgroepWaardelijsten
                    .Where(x => x.GebruikersgroepId == id)
                    .Select(x => x.WaardelijstId)
                    .AsEnumerable()
            };
        }
    }
}
