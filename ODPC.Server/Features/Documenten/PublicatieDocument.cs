namespace ODPC.Features.Documenten
{
    public class PublicatieDocument
    {
        public Guid Uuid { get; set; }
        public Guid Publicatie { get; set; }
        public required string OfficieleTitel { get; set; }
        public required Documenthandeling Documenthandeling { get; set; }
        public string? VerkorteTitel { get; set; }
        public string? Omschrijving { get; set; }
        public DateTime Creatiedatum { get; set; }
        public required string Bestandsnaam { get; set; }
        public required string Bestandsformaat { get; set; }
        public required double Bestandsomvang { get; set; }
        public List<Bestandsdeel>? Bestandsdelen { get; set; }
        public string? Status { get; set; }
    }

    public class Bestandsdeel
    {
        public required string Url { get; set; }
        public required int Volgnummer { get; set; }
        public required double Omvang { get; set; }
    }

    public class Documenthandeling
    {
        public required string SoortHandeling { get; set; }
        public required DateOnly Tijdstip { get; set; }
    }
}
