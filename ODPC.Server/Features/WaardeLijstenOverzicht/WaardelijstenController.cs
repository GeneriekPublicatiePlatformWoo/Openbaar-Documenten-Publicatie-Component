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
            // Deserialization of types without a parameterless constructor, a singular parameterized constructor, or a parameterized constructor annotated with 'JsonConstructorAttribute' is not supported. Type 'ODPC.Features.WaardeLijstenOverzicht.WaardelijstViewModel'.

            //infocategorien ophalen uit het ODRC

            var client = clientFactory.Create(user, "Waardelijsten ophalen");

            var response = await client.GetAsync("/api/v1/informatiecategorieen/", new CancellationToken());

            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();

            var deserializedResponse = JsonSerializer.Deserialize<PagedResponseModel<WaardelijstResponseModel>>(responseBody, JsonSerialization.Options);

            var results = deserializedResponse?.results ?? [];

            //alle wardelijsten zitten in 1 response. wellicht splitsten?
            //op dit moment worden alleen de inormatiecageroien opgehaald. voor nu aanvullen met wat mock data
            //viewModel.Add(new ThemaViewModel() { Id = "d3da5277-ea07-4921-97b8-e9a181390c76", Name = "arbeidsomstandigheden" });
            //viewModel.Add(new ThemaViewModel() { Id = "066241fe-7f39-41da-8efb-a702ef32b7d0", Name = "afval"  });
            //viewModel.Add(new OrganisatieViewModel() { Id = "8f939b51-dad3-436d-a5fa-495b42317d64", Name = "Organisatie 2"});
            //viewModel.Add(new OrganisatieViewModel () { Id = "5c14e7e2-00a2-4990-adbb-7290cd89fb6e", Name = "Organisatie 3" });
            //viewModel.Add(new ThemaViewModel() { Id = "0e7a0023-423a-421a-8700-359232fef584", Name = "europese zaken" });

            var viewModel = results.Select(x=> new WaardelijstViewModel { Id = x.Id, Name = x.Name}).ToList();



            viewModel.Add(new WaardelijstViewModel() { Id = "d3da5277-ea07-4921-97b8-e9a181390c76", Name = "arbeidsomstandigheden", Type = "THEMA" });
            viewModel.Add(new WaardelijstViewModel() { Id = "066241fe-7f39-41da-8efb-a702ef32b7d0", Name = "afval", Type = "THEMA" });

            viewModel.Add(new WaardelijstViewModel() { Id = "8f939b51-dad3-436d-a5fa-495b42317d64", Name = "Organisatie 2", Type="ORGANISATIE" });
            viewModel.Add(new WaardelijstViewModel() { Id = "5c14e7e2-00a2-4990-adbb-7290cd89fb6e", Name = "Organisatie 3", Type = "ORGANISATIE" });
            viewModel.Add(new WaardelijstViewModel() { Id = "0e7a0023-423a-421a-8700-359232fef584", Name = "europese zaken", Type = "THEMA" });
            return viewModel;
        }


    }
}
