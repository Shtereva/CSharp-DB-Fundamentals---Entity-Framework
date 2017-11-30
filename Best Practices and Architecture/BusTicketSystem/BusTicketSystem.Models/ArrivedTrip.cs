using System;

namespace BusTicketSystem.Models
{
    public class ArrivedTrip
    {
        public int Id { get; set; }

        public DateTime ActualArrivedTime { get; set; }

        public string OriginBusStation { get; set; }

        public string DestinationBusStation { get; set; }

        public int PassengersCount { get; set; }
    }
}
