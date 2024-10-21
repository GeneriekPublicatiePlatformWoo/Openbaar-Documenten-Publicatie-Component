using System.Runtime.CompilerServices;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ODPC.Apis.Odrc;
using ODPC.Authentication;
using ODPC.Data;

namespace ODPC.Features.Informatiecategorieen.MijnInformatiecategorieen
{
    [ApiController]
    public class MijnInformatiecategorieenController(OdpcUser user, IOdrcClientFactory clientFactory, OdpcDbContext context) : ControllerBase
    {
        [HttpGet("api/v1/mijn-informatiecategorieen")]
        public async IAsyncEnumerable<JsonObject> Get([EnumeratorCancellation] CancellationToken token)
        {
            var groepIds = context.GebruikersgroepGebruikers
                .Where(x => x.GebruikerId == user.Id)
                .Select(x => x.GebruikersgroepUuid);

            var categorieen = await context.GebruikersgroepWaardelijsten
                .Where(x => groepIds.Contains(x.GebruikersgroepUuid))
                .Select(x => x.WaardelijstId)
                .Distinct()
                .ToListAsync(token);

            if (categorieen.Count == 0) yield break;

            using var client = clientFactory.Create(user, "Eigen informatiecategorieen ophalen");
            var url = "api/v1/informatiecategorieen";
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
                url = page?.Next;
            }
        }
    }
}
