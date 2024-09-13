using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ODPC.Features.Gebruikersgroepen;

namespace ODPC.Features.WaardeLijstenOverzicht
{
    [Route("api/[controller]")]
    [ApiController]
    public class WaardelijstenController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<WaardelijstViewModel> Get()
        {
            //hoe de waardelijsten api eruit komt te zien is nog onbekend. mogelijk wordt dit tzt losse endpoints per soort lijst, mogelijk worden de id's url's, mogelijk verschilt het model per soort lijst....
            //gezien alle onbekendheden kiezen we even voor een zo simpel mogelijke start opzet
            return new WaardelijstViewModel[] {
                new() { Id = "d3da5277-ea07-4921-97b8-e9a181390c76", Name="arbeidsomstandigheden", Type="THEMA" },
                new() { Id = "f1e4632d-9c71-4ea4-9048-a5665671bbe4", Name="wet of algemeen bindend voorschrift", Type="INFORMATIECATEGORIE" },
                new() { Id = "066241fe-7f39-41da-8efb-a702ef32b7d0", Name = "afval", Type="THEMA" },
                new() { Id = "8f939b51-dad3-436d-a5fa-495b42317d64", Name = "Organisatie 2", Type="ORGANISATIE" },
                new() { Id = "5c14e7e2-00a2-4990-adbb-7290cd89fb6e", Name = "Organisatie 3", Type="ORGANISATIE" },
                new() { Id = "0e7a0023-423a-421a-8700-359232fef584", Name = "europese zaken", Type="THEMA" }
            };
        }
    }
}
