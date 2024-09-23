using Microsoft.AspNetCore.Mvc;

namespace ODPC.Features.Publicaties.PublicatieBijwerken
{
    [ApiController]
    public class PublicatieBijwerkenController : ControllerBase
    {
        [HttpPut("api/v1/publicaties/{uuid}")]
        public IActionResult Put(Guid uuid, Publicatie publicatie)
        {
            PublicatiesMock.Publicaties[uuid] = publicatie;
            return Ok(publicatie);
        }
    }
}
