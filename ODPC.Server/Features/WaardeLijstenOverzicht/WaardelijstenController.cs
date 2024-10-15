using System.Text.Json;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using ODPC.Apis.Odrc;
using ODPC.Authentication;
using ODPC.Config;
using ODPC.Features.Publicaties;

namespace ODPC.Features.WaardeLijstenOverzicht
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class WaardelijstenController(OdpUser user, IOrdcClientFactory clientFactory) : ControllerBase
    {
        [HttpGet]
        public async Task<IEnumerable<WaardelijstViewModel>> GetAsync()
        {
            //infocategorien ophalen uit het ODRC

            //user en audit log handeling meegeven is misschien nog niet nodig, maar doet geen kwaad
            var client = clientFactory.Create(user, "Waardelijsten ophalen");

            var response = await client.GetAsync("/api/v1/informatiecategorieen/", new CancellationToken());

            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            
            var deserializedResponse = JsonSerializer.Deserialize<PagedResponseModel<WaardelijstResponseModel>>(responseBody, JsonSerialization.Options);

            var informatiecategorieen = deserializedResponse?.results ?? [];
 
            var wardelijstenViewModel = informatiecategorieen.Select(x=> new WaardelijstViewModel { Id = x.Id, Name = x.Name}).ToList();


            //nog wat dummy data toevoegen van andersoortige waardelijsten
            wardelijstenViewModel.Add(new WaardelijstViewModel() { Id = "d3da5277-ea07-4921-97b8-e9a181390c76", Name = "arbeidsomstandigheden", Type = "THEMA" });
            wardelijstenViewModel.Add(new WaardelijstViewModel() { Id = "066241fe-7f39-41da-8efb-a702ef32b7d0", Name = "afval", Type = "THEMA" });
            wardelijstenViewModel.Add(new WaardelijstViewModel() { Id = "8f939b51-dad3-436d-a5fa-495b42317d64", Name = "Organisatie 2", Type="ORGANISATIE" });
            wardelijstenViewModel.Add(new WaardelijstViewModel() { Id = "5c14e7e2-00a2-4990-adbb-7290cd89fb6e", Name = "Organisatie 3", Type = "ORGANISATIE" });
            wardelijstenViewModel.Add(new WaardelijstViewModel() { Id = "0e7a0023-423a-421a-8700-359232fef584", Name = "europese zaken", Type = "THEMA" });
           
            return wardelijstenViewModel;
        }

    }
}
