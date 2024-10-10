using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ODPC.Features.Publicaties.PublicatiesOverzicht
{
    [ApiController]
    public class PublicatiesOverzichtController : ControllerBase
    {
        [HttpGet("api/v1/publicaties")]
        public IActionResult Get(int page = 1, int pageSize = 5)
        {
            var items = PublicatiesMock.Publicaties.Values.OrderByDescending(x => x.Registratiedatum);

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
