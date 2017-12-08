using System.Collections.Generic;
using Stations.Models;

namespace Stations.DataProcessor.Dto.export
{
    public class CustomerCardDto
    {
        public string Name { get; set; }

        public CardType Type { get; set; }

        public ICollection<Ticket> BoughtTickets { get; set; } = new List<Ticket>();

    }
}
