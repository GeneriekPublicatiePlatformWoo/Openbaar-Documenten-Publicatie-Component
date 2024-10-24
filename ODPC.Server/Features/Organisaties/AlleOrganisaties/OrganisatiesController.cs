using Microsoft.AspNetCore.Mvc;
using ODPC.Apis.Odrc;

namespace ODPC.Features.Organisaties.AlleOrganisaties
{
    [ApiController]
    public class OrganisatiesController : ControllerBase
    {
        [HttpGet("api/v1/organisaties")]
        public IActionResult Get([FromQuery] string? page) => Ok(new PagedResponseModel<WaardelijstResponseModel>
        {
            Results = OrganisatiesMock.Organisaties.Values.ToList(),
            Count = OrganisatiesMock.Organisaties.Count,
        });
    }
}
