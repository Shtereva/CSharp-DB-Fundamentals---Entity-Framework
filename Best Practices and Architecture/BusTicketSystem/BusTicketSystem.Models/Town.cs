using System.Collections.Generic;

namespace BusTicketSystem.Models
{
    public class Town
    {
        private ICollection<Customer> people;
        private ICollection<BusStation> busStations;

        public Town()
        {
            this.People = new HashSet<Customer>();
            this.BusStations = new HashSet<BusStation>();
        }
        public int Id { get; set; }

        public string Name { get; set; }

        public string Country { get; set; }

        public ICollection<Customer> People
        {
            get => this.people;
            set { this.people = value; }
        }

        public ICollection<BusStation> BusStations
        {
            get => this.busStations;
            set { this.busStations = value; }
        }
    }
}
