using System;
using System.Linq;
using BusTicketSystem.Data;
using BusTicketSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace BusTicketSystem.Client.Core.Commands
{
    public class BuyTicketCommand
    {
        public string Execute(string[] data)
        {
            if (data.Length != 4)
            {
                throw new InvalidOperationException("Invalid parameters!");
            }

            int customerId = int.Parse(data[0]);
            int tripId = int.Parse(data[1]);
            decimal price = decimal.Parse(data[2]);
            string seat = data[3];

            string result = string.Empty;

            using (var db = new BusTicketSystemContext())
            {
                var customer = db.Customers.Where(c => c.Id == customerId).Include(c => c.BankAccount).SingleOrDefault();

                if (customer == null || !db.Trips.Any(t => t.Id == tripId))
                {
                    throw new InvalidOperationException("There is no such customer or trip!");
                }

                if (db.Tickets.Any(t => t.CustomerId == customerId && t.TripId == tripId))
                {
                    throw new ArgumentException($"You have already bought ticket for  trip {tripId}");
                }

                if (price <= 0)
                {
                    throw new ArgumentException("Invalid price!");
                }

                if (customer.BankAccount.Balance - price < 0)
                {
                    throw new ArgumentException("Insufficient funds!");
                }

                customer.BankAccount.Balance -= price;

                var ticket = new Ticket()
                {
                    CustomerId = customerId,
                    Price = price,
                    Seat = seat,
                    TripId = tripId
                };

                db.Tickets.Add(ticket);
                db.SaveChanges();

                result = $"Customer {customer.FirstName} {customer.LastName} bought ticket for trip {tripId} for ${price} on seat {seat}";
            }

            return result;
        }
    }
}
