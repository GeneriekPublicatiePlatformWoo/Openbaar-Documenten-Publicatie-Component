using Microsoft.AspNetCore.Mvc;

namespace ODPC.Features.Publicaties.PublicatieDetails
{
    [ApiController]
    public class PublicatieDetailsController : ControllerBase
    {
        [HttpGet("api/v1/publicaties/{uuid}")]
        public IActionResult Get(Guid uuid)
        {
            return PublicatiesMock.Publicaties.TryGetValue(uuid, out var publicatie)
                ? Ok(publicatie)
                : NotFound();
        }
    }
}
