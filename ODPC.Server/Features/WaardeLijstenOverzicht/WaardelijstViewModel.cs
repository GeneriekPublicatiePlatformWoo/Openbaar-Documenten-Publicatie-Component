using System.ComponentModel;
using System.Text.Json.Serialization;

namespace ODPC.Features.WaardeLijstenOverzicht
{
    public  class WaardelijstViewModel
    {
        
        public required string Id { get; set; }
    
        public required string Name { get; set; }
   
        public string Type { get; set; } = "INFORMATIECATEGORIE"; //tijdelijk. als de andere categorien ook uit het odrc gehaald worden, zullen de waardelijsten volledig gesplitst worden
    }
}
