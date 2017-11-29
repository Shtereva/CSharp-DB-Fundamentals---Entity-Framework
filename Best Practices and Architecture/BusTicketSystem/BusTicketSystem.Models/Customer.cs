using System;
using System.Collections.Generic;

namespace BusTicketSystem.Models
{
    public class Customer
    {
        private ICollection<Review> reviews;

        public Customer()
        {
            this.Reviews = new HashSet<Review>();
        }
        public int Id { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime? BirthDate { get; set; }

        public Gender Gender { get; set; }

        public int HomeTownId{ get; set; }
        public Town HomeTown { get; set; }

        public int TicketId{ get; set; }
        public Ticket Ticket{ get; set; }

        public int BankAccountId { get; set; }
        public BankAccount BankAccount { get; set; }

        public ICollection<Review> Reviews
        {
            get => this.reviews;
            set { this.reviews = value; }
        }
    }
}
