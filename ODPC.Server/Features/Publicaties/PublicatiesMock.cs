using Microsoft.AspNetCore.Mvc;

namespace ODPC.Features.Publicaties
{
    public class Publicatie
    {
        public Guid Uuid { get; set; }
        public string? OfficieleTitel { get; set; }
        public string? VerkorteTitel { get; set; }
        public string? Omschrijving { get; set; }
        public DateOnly Creatiedatum { get; set; }
        public string? Status { get; set; }
    }

    [ApiController]
    [Route("api/v1/publicaties")]
    public class PublicatiesMock: ControllerBase
    {
        private static readonly Dictionary<Guid, Publicatie> s_publicaties = new Publicatie[] 
        {
            new() 
            { 
                Uuid = Guid.NewGuid(),
                OfficieleTitel = "Openbaarheid en Verantwoording: De Impact van de Wet open overheid op Bestuurlijke Transparantie",
                VerkorteTitel = "Openbaarheid en Verantwoording",
                Omschrijving = "",
                Creatiedatum = new DateOnly(2024, 08, 24)
            },
            new()
            {
                Uuid = Guid.NewGuid(),
                OfficieleTitel = "Inzicht voor Iedereen: Toepassing en Resultaten van de Wet open overheid",
                VerkorteTitel = "Inzicht voor Iedereen",
                Omschrijving = "",
                Creatiedatum = new DateOnly(2024, 05, 02)
            }
        }.ToDictionary(x=> x.Uuid);

        [HttpGet]
        public IActionResult AllPublicaties() => Ok(s_publicaties.Values);

        [HttpGet("{uuid}")]
        public IActionResult Single(Guid uuid) => s_publicaties.TryGetValue(uuid, out var publicatie) 
            ? Ok(publicatie) 
            : NotFound();

        [HttpPost]
        public IActionResult Post(Publicatie publicatie)
        {
            publicatie.Uuid = Guid.NewGuid();
            publicatie.Creatiedatum = DateOnly.FromDateTime(DateTime.Now);
            s_publicaties[publicatie.Uuid] = publicatie;
            return Ok(publicatie);
        }

        [HttpPut("{uuid}")]
        public IActionResult Put(Guid uuid, Publicatie publicatie)
        {
            s_publicaties[uuid] = publicatie;
            return Ok(publicatie);
        }

        [HttpDelete("{uuid}")]
        public IActionResult Delete(Guid uuid) 
        {
            s_publicaties.Remove(uuid);
            return NoContent();
        }
    }
}
