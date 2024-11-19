using System.Runtime.CompilerServices;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ODPC.Apis.Odrc;

namespace ODPC.Features.Informatiecategorieen.MijnInformatiecategorieen
{
    [ApiController]
    public class MijnInformatiecategorieenController(IOdrcClientFactory clientFactory, IGebruikerWaardelijstItemsService waardelijstItemsService) : ControllerBase
    {
        [HttpGet("api/{apiVersion}/mijn-informatiecategorieen")]
        public async IAsyncEnumerable<JsonObject> Get(string apiVersion, [EnumeratorCancellation] CancellationToken token)
        {
            var categorieen = await waardelijstItemsService.GetAsync(token);

            if (categorieen.Count == 0) yield break;

            using var client = clientFactory.Create("Eigen informatiecategorieen ophalen");
            var url = "/api/" + apiVersion + "/informatiecategorieen";

            // omdat we zelf moeten filteren obv van de waardelijstitems waar de gebruiker toegang toe heeft,
            // kunnen we geen paginering gebruiker. we lopen door alle pagina's van de ODRC
            while (!string.IsNullOrWhiteSpace(url))
            {
                var page = await client.GetFromJsonAsync<PagedResponseModel<JsonObject>>(url, token) ?? new() { Results = [] };

                foreach (var item in page.Results)
                {
                    if (item["uuid"]?.GetValue<string>() is string uuid && categorieen.Contains(uuid))
                    {
                        yield return item;
                    }
                }

                url = UrlHelper.GetPathAndQuery(page?.Next);
            }
        }
    }
}
