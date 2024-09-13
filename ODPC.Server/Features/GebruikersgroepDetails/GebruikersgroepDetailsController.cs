using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ODPC.Data;
using ODPC.Features.Gebruikersgroepen;

namespace ODPC.Features.GebruikersgroepDetails
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


        [HttpGet("api/gebruikersgroep/")]
        public GebruikersgroepDetailsModel Get(Guid id)
        {
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
