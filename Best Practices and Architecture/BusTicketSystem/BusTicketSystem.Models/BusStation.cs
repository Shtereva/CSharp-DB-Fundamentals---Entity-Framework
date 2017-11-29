using System.Collections.Generic;

namespace BusTicketSystem.Models
{
    public class BusStation
    {
        private ICollection<Trip> departures;
        private ICollection<Trip> arrivals;

        public BusStation()
        {
            this.Departures = new HashSet<Trip>();
            this.Arrivals = new HashSet<Trip>();
        }
        public int Id { get; set; }

        public string Name { get; set; }

        public int TownId { get; set; }
        public Town Town { get; set; }

        public ICollection<Trip> Departures
        {
            get => this.departures;
            set { this.departures = value; }
        }

        public ICollection<Trip> Arrivals
        {
            get => this.arrivals;
            set { this.arrivals = value; }
        }
    }
}
