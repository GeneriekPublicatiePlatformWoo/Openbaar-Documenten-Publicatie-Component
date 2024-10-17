using Microsoft.AspNetCore.Mvc;

namespace ODPC.Features.Documenten.InitialiseerDocument
{
    [ApiController]
    public class InitialiseerDocumentController : ControllerBase
    {
        [HttpPost("api/v1/documenten")]
        public IActionResult Post(PublicatieDocument document)
        {
            document.Uuid = Guid.NewGuid();
            document.Creatiedatum = DateTime.Now;

            var halve = document.Bestandsomvang / 2;
            var firstSize = Math.Ceiling(halve);
            var secondSize = Math.Floor(halve);

            document.Bestandsdelen =
            [
                new()
                {
                    Omvang = firstSize,
                    Url = $"/api/v1/documenten/{document.Uuid}/bestandsdelen/1",
                    Volgnummer = 0
                },
                new()
                {
                    Omvang = secondSize,
                    Url = $"/api/v1/documenten/{document.Uuid}/bestandsdelen/2",
                    Volgnummer = 1
                }
            ];

            DocumentenMock.Documenten[document.Uuid] = document;
            return Ok(document);
        }
    }
}
