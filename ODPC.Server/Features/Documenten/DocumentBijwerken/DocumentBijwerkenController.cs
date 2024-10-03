using Microsoft.AspNetCore.Mvc;

namespace ODPC.Features.Documenten.DocumentBijwerken
{
    [ApiController]
    public class DocumentBijwerkenController : ControllerBase
    {
        [HttpPut("api/v1/documenten/{uuid:guid}")]
        public IActionResult Put(Guid uuid, PublicatieDocument document)
        {
            DocumentenMock.Documenten[document.Uuid] = document;
            return Ok(document);
        }
    }
}
