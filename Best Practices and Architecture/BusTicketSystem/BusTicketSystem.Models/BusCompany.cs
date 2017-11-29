using System.Collections.Generic;
using System.Linq;

namespace BusTicketSystem.Models
{
    public class BusCompany
    {
        public int Id { get; set; }

        private ICollection<Trip> trips;
        private ICollection<Review> reviews;

        public BusCompany()
        {
            this.Trips = new HashSet<Trip>();
            this.Reviews = new HashSet<Review>();
        }

        public string Name { get; set; }

        public string Nationality { get; set; }

        public double Rating { get; set; }
        public ICollection<Trip> Trips
        {
            get => this.trips;
            set { this.trips = value; }
        }

        public ICollection<Review> Reviews
        {
            get => this.reviews;
            set { this.reviews = value; }
        }

    }
}
