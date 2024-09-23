using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ODPC.Data;
using ODPC.Data.Entities;

namespace ODPC.Features.AuditregelOverzicht
{
    [ApiController]
    public class AuditregelsController(OdpcDbContext context, CurrentRequestUri currentUri) : ControllerBase
    {
        const int PageSize = 15;

        [HttpGet("api/auditregels")]
        public async Task<PaginatedModel<AuditregelOverzichtModel>> Get([FromQuery] AuditregelOverzichtRequest request, CancellationToken token)
        {
            var page = request.Page ?? 1;

            var filteredRegels = ApplyFilters(request, context.Auditregels);

            var count = await filteredRegels.CountAsync(token);

            var records = await filteredRegels
                .OrderByDescending(x => x.Aanmaakdatum)
                .Skip(PageSize * (page - 1))
                .Take(PageSize)
                .Select(x => new AuditregelOverzichtModel
                {
                    ResourceUuid = x.ResourceUuid,
                    Resource = x.Resource,
                    GebruikersId = x.GebruikersId,
                    Actie = x.Actie,
                    ActieWeergave = x.ActieWeergave,
                    ResultaatWeergave = x.ResultaatWeergave,
                    Geslaagd = x.Geslaagd,
                    Aanmaakdatum = x.Aanmaakdatum,
                    Wijziging = new AuditregelOverzichtWijzingModel { Oud = x.Wijzigingen.Oud, Nieuw = x.Wijzigingen.Nieuw },
                    Uuid = x.Uuid,
                    ResourceWeergave = x.ResourceWeergave,
                    Resultaat = x.Resultaat,
                })
                .ToListAsync(token);

            var (previous, next) = GetPages(currentUri, count, page);

            var result = new PaginatedModel<AuditregelOverzichtModel> { Count = count, Results = records, Previous = previous, Next = next };

            return result;
        }

        private static IQueryable<Auditregel> ApplyFilters(AuditregelOverzichtRequest request, IQueryable<Auditregel> queryable)
        {
            if (request.Geslaagd != null)
            {
                queryable = queryable.Where(x => x.Geslaagd == request.Geslaagd.Value);
            }

            if (!string.IsNullOrWhiteSpace(request.GebruikersId))
            {
                queryable = queryable.Where(x => x.GebruikersId == request.GebruikersId);
            }

            if (request.ResourceUuid != null)
            {
                queryable = queryable.Where(x => x.ResourceUuid == request.ResourceUuid.Value);
            }

            return queryable;
        }

        private static (Uri? Previous, Uri? Next) GetPages(CurrentRequestUri currentUri, int count, int page)
        {
            var uriBuilder = new UriBuilder(currentUri);
            var query = HttpUtility.ParseQueryString(currentUri.Query);
            Uri? previous = null;
            Uri? next = null;

            // als er meer pagina's zijn
            if (count > PageSize * page)
            {
                query["page"] = (page + 1).ToString();
                uriBuilder.Query = query.ToString();
                next = uriBuilder.Uri;
            }

            // als er een vorige pagina is
            if (page > 1)
            {
                query["page"] = (page - 1).ToString();
                uriBuilder.Query = query.ToString();
                previous = uriBuilder.Uri;
            }

            return (previous, next);
        }
    }
}
