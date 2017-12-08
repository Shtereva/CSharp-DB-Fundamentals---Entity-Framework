using System;
using Stations.Models;

namespace Stations.DataProcessor.Dto.export
{
    public class TripDto
    {
        public string OriginStation { get; set; }

        public string DestinationStation { get; set; }

        public DateTime DepartureTime { get; set; }
    }
}
