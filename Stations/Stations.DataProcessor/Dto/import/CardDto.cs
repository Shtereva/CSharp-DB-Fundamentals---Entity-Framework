using System.ComponentModel.DataAnnotations;
using Stations.Models;

namespace Stations.DataProcessor.Dto.import
{
    public class CardDto
    {
        [Required]
        [MaxLength(128)]
        public string Name { get; set; }

        [Range(0, 120)]
        public int? Age { get; set; }

        public string Type { get; set; } = "Normal";
    }
}
