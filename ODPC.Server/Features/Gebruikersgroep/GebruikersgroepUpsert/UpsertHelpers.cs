using Microsoft.EntityFrameworkCore;
using ODPC.Data;
using ODPC.Data.Entities;

namespace ODPC.Features.Gebruikersgroep.GebruikersgroepUpsert
{
    public class UpsertHelpers
    {
        //voeg de nieuwe set waardelijsten toe aan deze groep
        public static void AddWaardelijstenToGroep(List<string> gekoppeldeWaardelijsten, Data.Entities.Gebruikersgroep groep, OdpcDbContext context)
        {
            context.GebruikersgroepWaardelijsten
                .AddRange(gekoppeldeWaardelijsten
                    .Select(x => new GebruikersgroepWaardelijst { Gebruikersgroep = groep, WaardelijstId = x }));
        }
    }
}
