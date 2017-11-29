using System;
using System.Linq;
using System.Text;
using BusTicketSystem.Data;
using Microsoft.EntityFrameworkCore;

namespace BusTicketSystem.Client.Core.Commands
{
    public class PrintInfoCommand
    {
        public string Execute(string[] data)
        {
            if (data.Length != 1)
            {
                throw new InvalidOperationException("Invalid parameters!");
            }

            int busId = int.Parse(data[0]);

            var sb = new StringBuilder();

            using (var db = new BusTicketSystemContext())
            {
                var busStation = db.BusStations
                    .Where(b => b.Id == busId)
                    .Select(b => new
                    {
                        StationName = b.Name,
                        Town = b.Town.Name,
                        Arrivals = b.Arrivals.Select(a => new
                        {
                            OriginStation = a.OriginBusStation.Name,
                            ArrivalTime = a.ArrivalTime,
                            Status = a.Status
                        }).ToList(),
                        Departures = b.Departures.Select(a => new
                        {
                            DestinationStation = a.DestinationBusStation.Name,
                            DepartureTime = a.DepartureTime,
                            Status = a.Status
                        }).ToList()
                    }).SingleOrDefault();

                if (busStation == null)
                {
                    throw new ArgumentException("There is no such Bus Station!");
                }
                sb.AppendLine($"{busStation.StationName}, {busStation.Town}");

                var arrivals = busStation.Arrivals
                    .Select(a => $"From {a.OriginStation} | Arrive at: {a.ArrivalTime} | Status: {a.Status}")
                    .ToList();

                sb.AppendLine($"Arrivals:{Environment.NewLine}{string.Join(Environment.NewLine, arrivals)}");

                var departures = busStation.Departures
                    .Select(d => $"To {d.DestinationStation} | Depart at: {d.DepartureTime} | Status {d.Status}")
                    .ToList();

                sb.AppendLine($"Departures:{Environment.NewLine}{string.Join(Environment.NewLine, departures)}");
            }

            return sb.ToString();
        }
    }
}
