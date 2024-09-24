using Microsoft.AspNetCore.Mvc;

namespace ODPC.Features.Documenten.UploadBestandsdeel
{
    [ApiController]
    public class UploadBestandsdeelController : ControllerBase
    {
        [HttpPut("api/v1/documenten/{uuid:guid}/bestandsdelen/{volgnummer:int}")]
        public IActionResult Put(Guid uuid, int volgnummer)
        {
            return NoContent();
        }
    }
}
