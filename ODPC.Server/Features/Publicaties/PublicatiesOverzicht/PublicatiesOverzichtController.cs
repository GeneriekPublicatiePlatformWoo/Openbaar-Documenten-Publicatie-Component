using Microsoft.AspNetCore.Mvc;

namespace ODPC.Features.Publicaties.PublicatiesOverzicht
{
    [ApiController]
    public class PublicatiesOverzichtController : ControllerBase
    {
        [HttpGet("api/v1/publicaties")]
        public IActionResult Get()
        {
            return Ok(PublicatiesMock.Publicaties.Values.OrderByDescending(x => x.Creatiedatum));
        }
    }
}
