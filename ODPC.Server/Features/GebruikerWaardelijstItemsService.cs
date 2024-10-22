using Microsoft.EntityFrameworkCore;
using ODPC.Authentication;
using ODPC.Data;

namespace ODPC.Features
{
    public interface IGebruikerWaardelijstItemsService
    {
        Task<IReadOnlyList<string>> GetAsync(CancellationToken token);
    }

    public class GebruikerWaardelijstItemsService(OdpcUser user, OdpcDbContext context) : IGebruikerWaardelijstItemsService
    {
        public async Task<IReadOnlyList<string>> GetAsync(CancellationToken token)
        {
            var groepIds = context.GebruikersgroepGebruikers
                .Where(x => x.GebruikerId == user.Id)
                .Select(x => x.GebruikersgroepUuid);

            return await context.GebruikersgroepWaardelijsten
                .Where(x => groepIds.Contains(x.GebruikersgroepUuid))
                .Select(x => x.WaardelijstId)
                .Distinct()
                .ToListAsync(token);
        }
    }
}
