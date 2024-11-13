using System.Runtime.CompilerServices;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Mvc;
using ODPC.Apis.Odrc;

namespace ODPC.Features.Organisaties.MijnOrganisaties
{
    public class MijnOrganisatiesController(IOdrcClientFactory clientFactory, IGebruikerWaardelijstItemsService waardelijstItemsService) : ControllerBase
    {
        [HttpGet("api/v1/mijn-organisaties")]
        public async IAsyncEnumerable<JsonObject> Get([EnumeratorCancellation] CancellationToken token)
        {
            var organisaties = await waardelijstItemsService.GetAsync(token);

            if (organisaties.Count == 0) yield break;

            using var client = clientFactory.Create("Eigen organisaties ophalen");
            var url = "api/v1/organisaties";

            // omdat we zelf moeten filteren obv van de waardelijstitems waar de gebruiker toegang toe heeft,
            // kunnen we geen paginering gebruiker. we lopen door alle pagina's van de ODRC
            while (!string.IsNullOrWhiteSpace(url))
            {
                var page = await client.GetFromJsonAsync<PagedResponseModel<JsonObject>>(url, token) ?? new() { Results = [] };
                foreach (var item in page.Results)
                {
                    if (item["uuid"]?.GetValue<string>() is string uuid && organisaties.Contains(uuid))
                    {
                        yield return item;
                    }
                }
                url = page?.Next;
            }
        }
    }
}
