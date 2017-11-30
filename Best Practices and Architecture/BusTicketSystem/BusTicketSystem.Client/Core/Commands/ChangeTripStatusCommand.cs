using System;
using System.Linq;
using System.Text;
using BusTicketSystem.Data;
using BusTicketSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace BusTicketSystem.Client.Core.Commands
{
    public class ChangeTripStatusCommand
    {
        public string Execute(string[] data)
        {
            if (data.Length != 2)
            {
                throw new InvalidOperationException("Invalid parameters!");
            }

            int tripId = int.Parse(data[0]);
            string newStatus = data[1];
            var result = new StringBuilder();

            using (var db = new BusTicketSystemContext())
            {
                var trip = db.Trips
                    .Include(t => t.OriginBusStation)
                    .ThenInclude(s => s.Town)
                    .Include(t => t.DestinationBusStation)
                    .ThenInclude(s => s.Town)
                    .Include(t => t.Tickets)
                    .SingleOrDefault(t => t.Id == tripId);

                if (trip == null)
                {
                    throw new InvalidOperationException("There is no such trip!");
                }

                var statusNames = Enum.GetNames(typeof(Status)).ToList();

                if (!statusNames.Contains(newStatus))
                {
                    throw new InvalidOperationException("Invalid status name!");
                }

                string oldStatus = trip.Status.ToString();

                trip.Status = (Status)statusNames.IndexOf(newStatus);

                string originTown = trip.OriginBusStation.Town.Name;
                string destinationTown = trip.DestinationBusStation.Town.Name;

                result.AppendLine($"Trip from {originTown} to {destinationTown} on {trip.DepartureTime}");

                result.AppendLine($"Status changed from {oldStatus} to {newStatus}");

                if (newStatus == "Arrived")
                {
                    int passengersCount = trip.Tickets.Count;

                    result.Append($"On {trip.ArrivalTime} - {passengersCount} passengers arrived at {destinationTown} from {originTown}");

                    var arrivedTrip = new ArrivedTrip()
                    {
                        ActualArrivedTime = trip.ArrivalTime,
                        DestinationBusStation = destinationTown,
                        OriginBusStation = originTown,
                        PassengersCount = passengersCount
                    };

                    db.ArrivedTrips.Add(arrivedTrip);
                }

                db.SaveChanges();
            }

            return result.ToString();
        }
    }
}
