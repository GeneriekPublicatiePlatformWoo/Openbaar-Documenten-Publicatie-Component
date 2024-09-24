using Microsoft.AspNetCore.Mvc;

namespace ODPC.Features.Documenten.DocumentDetail
{
    [ApiController]
    public class DocumentDetailController: ControllerBase
    {
        [HttpGet("api/v1/documenten/{uuid:guid}")]
        public IActionResult Get(Guid uuid)
        {
            return DocumentenMock.Documenten.TryGetValue(uuid, out var document)
                ? Ok(document)
                : NotFound();
        }
    }
}
