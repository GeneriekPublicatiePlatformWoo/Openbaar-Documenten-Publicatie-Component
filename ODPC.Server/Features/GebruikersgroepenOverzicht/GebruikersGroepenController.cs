using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ODPC.Controllers;
using ODPC.Data;
using ODPC.Features.Gebruikersgroep.GebruikersgroepDetails;
using ODPC.Features.Gebruikersgroepen;

namespace ODPC.Features.GebruikersgroepenOverzicht
{
    [Route("api/[controller]")]
    [ApiController]
    public class GebruikersGroepenController(OdpcDbContext context, ILogger<GebruikersgroepDetailsController> logger) : ControllerBase
    {

        private readonly OdpcDbContext _context = context;
        private readonly ILogger<GebruikersgroepDetailsController> _logger = logger;

        [HttpGet]
        public IEnumerable<GebruikersgroepModel> Get()
        {
            return _context
              .Gebruikersgroepen
              .OrderBy(x => x.Name)
              .Select(x=> new GebruikersgroepModel { Name = x.Name, Id = x.Id })
              .AsEnumerable();
        }
    }
}
