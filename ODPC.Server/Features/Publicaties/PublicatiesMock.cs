namespace ODPC.Features.Publicaties
{
    public static class PublicatiesMock
    {
        public static readonly Dictionary<Guid, Publicatie> Publicaties = new Publicatie[]
        {
            new()
            {
                Uuid = Guid.NewGuid(),
                OfficieleTitel = "Openbaarheid en Verantwoording: De Impact van de Wet open overheid op Bestuurlijke Transparantie",
                VerkorteTitel = "Openbaarheid en Verantwoording",
                Omschrijving = "",
                Creatiedatum = new DateOnly(2024, 08, 24)
            },
            new()
            {
                Uuid = Guid.NewGuid(),
                OfficieleTitel = "Inzicht voor Iedereen: Toepassing en Resultaten van de Wet open overheid",
                VerkorteTitel = "Inzicht voor Iedereen",
                Omschrijving = "",
                Creatiedatum = new DateOnly(2024, 05, 02)
            }
        }.ToDictionary(x => x.Uuid);
    }
}
