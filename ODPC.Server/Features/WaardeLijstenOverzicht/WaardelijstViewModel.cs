using System.ComponentModel;
using System.Text.Json.Serialization;

namespace ODPC.Features.WaardeLijstenOverzicht
{
    public  class WaardelijstViewModel
    {
        
        public required string Id { get; set; }
    
        public required string Name { get; set; }

   
        public string Type { get; set; } = "INFORMATIECATEGORIE";
    }


    //public class InformatiecategorieViewModel : WaardelijstViewModel
    //{     
    //    public override string Type => "INFORMATIECATEGORIE";
    //}

    //public class ThemaViewModel : WaardelijstViewModel
    //{
    //    public override string Type => "THEMA";
    //}

    //public class OrganisatieViewModel : WaardelijstViewModel
    //{
    //    public override string Type => "ORGANISATIE";
    //} 

}
