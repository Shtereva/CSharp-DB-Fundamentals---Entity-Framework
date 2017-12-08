using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Stations.Data;
using Stations.DataProcessor.Dto.export;
using Stations.Models;

namespace Stations.DataProcessor
{
	public class Serializer
	{
		public static string ExportDelayedTrains(StationsDbContext context, string dateAsString)
		{
		    var date = DateTime.ParseExact(dateAsString, "dd/MM/yyyy", CultureInfo.InvariantCulture);

		    var trains = context.Trains
		        .Where(t => t.Trips.Any(x => x.Status == TripStatus.Delayed && x.DepartureTime <= date))
		        .ProjectTo<TrainDto>(x => x.MaxDelayedTime.ToString())
		        .OrderByDescending(t => t.DelayedTimes)
		        .ThenByDescending(t => t.MaxDelayedTime.ToString())
		        .ThenBy(t => t.TrainNumber)
		        .ToArray();

		    string json = JsonConvert.SerializeObject(trains, Formatting.Indented);

		    return json;
		}

		public static string ExportCardsTicket(StationsDbContext context, string cardType)
		{
		    var type = Enum.GetNames(typeof(CardType)).ToList().IndexOf(cardType);

		    var cards = context.Cards
		        .Where(c => c.Type == (CardType)type && c.BoughtTickets.Any())
                .ProjectTo<CustomerCardDto>(x => x.Type.ToString())
		        .OrderBy(c => c.Name)
                .ToArray();

		    var xDoc = new XDocument();
            var xElementsCard = new List<XElement>();

		    foreach (var card in cards)
		    {
                
		        var xElementsTicket = new List<XElement>();

		        foreach (var ticket in card.BoughtTickets)
		        {
		            var trip = context.Trips.SingleOrDefault(t => t.Id == ticket.TripId);

		            string originStation = context.Stations.SingleOrDefault(s => s.Id == trip.OriginStationId).Name;
		            string destinationStation = context.Stations.SingleOrDefault(s => s.Id == trip.DestinationStationId).Name;

		            var date = trip.DepartureTime.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);


                    xElementsTicket.Add(new XElement("Ticket",
                                            new XElement("OriginStation", originStation),
                                            new XElement("DestinationStation", destinationStation),
                                            new XElement("DepartureTime", date)));
		        }

		        xElementsCard.Add(new XElement("Card",
		                             new XAttribute("name", card.Name),
		                             new XAttribute("type", card.Type),
		                             new XElement("Tickets", xElementsTicket)));

                
		    }

		    xDoc.Add(new XElement("Cards", xElementsCard));

            return xDoc.ToString();
		}
	}
}