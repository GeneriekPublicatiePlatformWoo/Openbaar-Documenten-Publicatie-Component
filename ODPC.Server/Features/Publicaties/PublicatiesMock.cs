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
                VerkorteTitel = "De Impact van de Wet open overheid op Bestuurlijke Transparantie",
                Omschrijving = "",
                Registratiedatum = new DateOnly(2024, 08, 24)
            },
            new()
            {
                Uuid = Guid.NewGuid(),
                OfficieleTitel = "Inzicht voor Iedereen: Toepassing en Resultaten van de Wet open overheid",
                VerkorteTitel = "Toepassing en Resultaten van de Wet open overheid",
                Omschrijving = "",
                Registratiedatum = new DateOnly(2024, 05, 02)
            },
            new()
            {
                Uuid = Guid.NewGuid(),
                OfficieleTitel = "Open Overheid: Transparantie als Standaard in Bestuurlijk Nederland",
                VerkorteTitel = "Transparantie als Standaard in Bestuurlijk Nederland",
                Omschrijving = "",
                Registratiedatum = new DateOnly(2024, 08, 24)
            },
            new()
            {
                Uuid = Guid.NewGuid(),
                OfficieleTitel = "De Wet open overheid: Een Nieuwe Norm voor Openbare Informatie",
                VerkorteTitel = "Een Nieuwe Norm voor Openbare Informatie",
                Omschrijving = "",
                Registratiedatum = new DateOnly(2024, 05, 02)
            },
            new()
            {
                Uuid = Guid.NewGuid(),
                OfficieleTitel = "Inzicht in Overheid: De Toekomst van Transparantie met de Woo",
                VerkorteTitel = "De Toekomst van Transparantie met de Woo",
                Omschrijving = "",
                Registratiedatum = new DateOnly(2024, 08, 24)
            },
            new()
            {
                Uuid = Guid.NewGuid(),
                OfficieleTitel = "Toegang tot Informatie: De Praktische Uitwerking van de Woo",
                VerkorteTitel = "De Praktische Uitwerking van de Woo",
                Omschrijving = "",
                Registratiedatum = new DateOnly(2024, 05, 02)
            },
            new()
            {
                Uuid = Guid.NewGuid(),
                OfficieleTitel = "Openbaarheid en Verantwoording: De Impact van de Wet open overheid op Bestuurlijke Transparantie",
                VerkorteTitel = "Openbaarheid en Verantwoording",
                Omschrijving = "",
                Registratiedatum = new DateOnly(2024, 08, 24)
            },
            new()
            {
                Uuid = Guid.NewGuid(),
                OfficieleTitel = "Verantwoording en Openheid: Hoe de Woo de Overheid Hervormt",
                VerkorteTitel = "Hoe de Woo de Overheid Hervormt",
                Omschrijving = "",
                Registratiedatum = new DateOnly(2024, 05, 02)
            },
            new()
            {
                Uuid = Guid.NewGuid(),
                OfficieleTitel = "De Wet open overheid in de Praktijk: Successen en Uitdagingen",
                VerkorteTitel = "Successen en Uitdagingen",
                Omschrijving = "",
                Registratiedatum = new DateOnly(2024, 08, 24)
            },
            new()
            {
                Uuid = Guid.NewGuid(),
                OfficieleTitel = "Publieke Toegang tot Overheidsinformatie: Het Effect van de Woo",
                VerkorteTitel = "Het Effect van de Woo",
                Omschrijving = "",
                Registratiedatum = new DateOnly(2024, 05, 02)
            },
            new()
            {
                Uuid = Guid.NewGuid(),
                OfficieleTitel = "De Kracht van Openbaarheid: Verantwoord Bestuur door de Wet open overheid",
                VerkorteTitel = "Verantwoord Bestuur door de Wet open overheid",
                Omschrijving = "",
                Registratiedatum = new DateOnly(2024, 08, 24)
            },
            new()
            {
                Uuid = Guid.NewGuid(),
                OfficieleTitel = "Transparantie als Fundament: De Woo en de Weg naar Open Overheid",
                VerkorteTitel = "De Woo en de Weg naar Open Overheid",
                Omschrijving = "",
                Registratiedatum = new DateOnly(2024, 05, 02)
            }
        }.ToDictionary(x => x.Uuid);
    }
}
