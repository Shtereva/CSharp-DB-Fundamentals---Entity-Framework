using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Stations.DataProcessor.Dto.import
{
    //[XmlType("Ticket")]
    public class TicketDto
    {
       // [XmlAttribute("price")]
        [Required]
        [Range(typeof(decimal), "0", "79228162514264337593543950335")]
        public decimal Price { get; set; }

        [Required]
        [RegularExpression(@"^[A-Z]{2}\d{1,6}$")]
        public string Seat { get; set; }

        public string OriginStation { get; set; }

        [Required]
        [MaxLength(50)]
        public string DestinationStation { get; set; }

        [Required]
        public string DepartureTime { get; set; }

        [MaxLength(128)]
        public string CardName { get; set; }
    }
}
