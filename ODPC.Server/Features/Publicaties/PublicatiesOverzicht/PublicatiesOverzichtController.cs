using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ODPC.Features.Publicaties.PublicatiesOverzicht
{
    [ApiController]
    public class PublicatiesOverzichtController : ControllerBase
    {
        [HttpGet("api/v1/publicaties")]
        public IActionResult Get(int page = 1, string sorteer = "registratiedatum", string search = "", string registratiedatum__gte = "", string registratiedatum__lte = "", int pageSize = 5)
        {
            var items = PublicatiesMock.Publicaties.Values.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                items = items.Where(x =>
                    (x.OfficieleTitel != null && x.OfficieleTitel.Contains(search, StringComparison.OrdinalIgnoreCase)) ||
                    (x.VerkorteTitel != null && x.VerkorteTitel.Contains(search, StringComparison.OrdinalIgnoreCase)));
            }

            DateTime? fromDate = null;
            DateTime? untilDate = null;

            if (!string.IsNullOrEmpty(registratiedatum__gte))
            {
                if (DateTime.TryParseExact(registratiedatum__gte, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime parsedFromDate))
                {
                    fromDate = parsedFromDate;
                }
            }

            if (!string.IsNullOrEmpty(registratiedatum__lte))
            {
                if (DateTime.TryParseExact(registratiedatum__lte, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime parsedUntilDate))
                {
                    untilDate = parsedUntilDate;
                }
            }

            // Apply date range filtering if 'from' and/or 'until' dates are provided
            if (fromDate.HasValue)
            {
                items = items.Where(x => x.Registratiedatum >= fromDate.Value);
            }

            if (untilDate.HasValue)
            {
                items = items.Where(x => x.Registratiedatum <= untilDate.Value);
            }

            switch (sorteer.ToLower())
            {
                case "officiele_titel":
                    items = items.OrderBy(x => x.OfficieleTitel ?? string.Empty);
                    break;
                case "-officiele_titel":
                    items = items.OrderByDescending(x => x.OfficieleTitel ?? string.Empty);
                    break;
                case "verkorte_titel":
                    items = items.OrderBy(x => x.VerkorteTitel);
                    break;
                case "-verkorte_titel":
                    items = items.OrderByDescending(x => x.VerkorteTitel);
                    break;
                case "registratiedatum":
                    items = items.OrderBy(x => x.Registratiedatum);
                    break;
                default:
                    items = items.OrderByDescending(x => x.Registratiedatum); // Default sorting
                    break;
            }

            // Calculate total count
            var totalCount = items.Count();

            // Paginate the items
            var pagedItems = items
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Build next and previous page URLs
            var nextPage = page * pageSize < totalCount ? $"api/v1/publicaties/?page={page + 1}" : null;
            var previousPage = page > 1 ? $"api/v1/publicaties/?page={page - 1}" : null;

            // Build the result
            var result = new
            {
                count = totalCount,
                next = nextPage,
                previous = previousPage,
                results = pagedItems
            };

            // Return the paged result
            return Ok(result);
        }
    }
}
