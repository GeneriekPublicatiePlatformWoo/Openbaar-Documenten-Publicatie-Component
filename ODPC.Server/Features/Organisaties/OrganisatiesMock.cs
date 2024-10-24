using ODPC.Apis.Odrc;

namespace ODPC.Features.Organisaties
{
    public class OrganisatiesMock
    {
        public static readonly Dictionary<string, WaardelijstResponseModel> Organisaties = new WaardelijstResponseModel[]
        {
            new() { Uuid = "8f939b51-dad3-436d-a5fa-495b42317d64", Naam = "Organisatie 2" },
            new() { Uuid = "5c14e7e2-00a2-4990-adbb-7290cd89fb6e", Naam = "Organisatie 3" }
        }.ToDictionary(x => x.Uuid);
    }
}
