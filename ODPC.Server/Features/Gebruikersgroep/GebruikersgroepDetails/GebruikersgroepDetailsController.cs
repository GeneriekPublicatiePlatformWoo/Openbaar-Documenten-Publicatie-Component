using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ODPC.Data;
using ODPC.Features.Gebruikersgroepen;

namespace ODPC.Features.Gebruikersgroep.GebruikersgroepDetails
{

    [ApiController]
    public class GebruikersgroepDetailsController : ControllerBase
    {
        private readonly OdpcDbContext _context;
        private readonly ILogger<GebruikersgroepDetailsController> _logger;

        public GebruikersgroepDetailsController(OdpcDbContext context, ILogger<GebruikersgroepDetailsController> logger)
        {
            _context = context;
            _logger = logger;
        }


        [HttpGet("api/gebruikersgroepen/{Id}")]
        public GebruikersgroepDetailsModel Get(Guid id)
        {
            var groep = _context.Gebruikersgroepen.Single(x => x.Id == id);

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
