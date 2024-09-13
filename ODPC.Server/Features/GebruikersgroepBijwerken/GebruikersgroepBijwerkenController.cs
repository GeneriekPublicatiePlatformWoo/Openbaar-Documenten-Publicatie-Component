using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ODPC.Data;
using ODPC.Data.Entities;
using ODPC.Features.GebruikersgroepDetails;

namespace ODPC.Features.GebruikersgroepBijwerken
{    
    [ApiController]
    public class GebruikersgroepBijwerkenController : ControllerBase
    {

        private readonly OdpcDbContext _context;
        private readonly ILogger<GebruikersgroepDetailsController> _logger;

        public GebruikersgroepBijwerkenController(OdpcDbContext context, ILogger<GebruikersgroepDetailsController> logger)
        {
            _context = context;
            _logger = logger;
        }


        [HttpPut("api/gebruikersgroep/{id}")]
        public GebruikersgroepDetailsModel Put(Guid id, GebruikersgroepDetailsModel model)
        {
            //verwijder bestaande waardelijsten voor deze groep
            var gebruikersGroepen = _context
                .GebruikersgroepWaardelijsten
                .Where(x => x.GebruikersgroepId == id);

            _context.GebruikersgroepWaardelijsten
                .RemoveRange(gebruikersGroepen);

            //voeg de nieuwe set waardelijsten toe aan deze groep
            var groep = _context.Gebruikersgroepen
                .Single(x => x.Id == id);

            _context.GebruikersgroepWaardelijsten
                .AddRange(model.BeschikbareWaardelijsten
                .Select(x => new GebruikersgroepWaardelijst { Gebruikersgroep = groep, WaardelijstId = x }));

            _context.SaveChanges();

            return new GebruikersgroepDetailsModel
            {
                BeschikbareWaardelijsten = _context
                    .GebruikersgroepWaardelijsten
                    .Where(x => x.GebruikersgroepId == id)
                    .Select(x => x.WaardelijstId)
                    .AsEnumerable()
            };
        }
    }
}
