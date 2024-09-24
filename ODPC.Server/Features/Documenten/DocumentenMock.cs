using ODPC.Features.Publicaties;

namespace ODPC.Features.Documenten
{
    public static class DocumentenMock
    {
        public static readonly Dictionary<Guid, PublicatieDocument> Documenten = new PublicatieDocument[]
        {
            new()
            {
                Uuid = Guid.NewGuid(),
                Publicatie = PublicatiesMock.Publicaties.Keys.First(),
                OfficieleTitel = "Belangrijk document",
                VerkorteTitel = "Document",
                Omschrijving = "",
                Creatiedatum = new DateOnly(2024, 09, 23),
                Bestandsformaat = "DOCX",
                Bestandsnaam = "belangrijk.docx",
                Bestandsomvang = 100000
            }
        }
        .ToDictionary(x => x.Uuid);
    }
}
