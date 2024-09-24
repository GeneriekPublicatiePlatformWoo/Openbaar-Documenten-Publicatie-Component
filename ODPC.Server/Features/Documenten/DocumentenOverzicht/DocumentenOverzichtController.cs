using Microsoft.AspNetCore.Mvc;

namespace ODPC.Features.Documenten.DocumentenOverzicht
{
    [ApiController]
    public class DocumentenOverzichtController : ControllerBase
    {
        [HttpGet("api/v1/documenten")]
        public IActionResult Get([FromQuery] Guid publicatie)
        {
            var documenten = DocumentenMock.Documenten.Values
                .Where(x=> x.Publicatie == publicatie)
                .ToList();

            return Ok(documenten);
        }
    }
}
