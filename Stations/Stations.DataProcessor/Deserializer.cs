using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Stations.Data;
using Stations.DataProcessor.Dto.import;
using Stations.Models;

namespace Stations.DataProcessor
{
    public static class Deserializer
    {
        private const string FailureMessage = "Invalid data format.";
        private const string SuccessMessage = "Record {0} successfully imported.";

        public static string ImportStations(StationsDbContext context, string jsonString)
        {
            var json = JsonConvert.DeserializeObject<List<StationDto>>(jsonString);

            var sb = new StringBuilder();

            foreach (var element in json)
            {
                string name = element.Name;
                string town = element.Town ?? element.Name;

                bool isValid = IsValid(element);

                if (!isValid || context.Stations.Any(s => s.Name == name))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                var station = new Station() { Name = name, Town = town };
                context.Stations.Add(station);
                context.SaveChanges();

                var msg = string.Format(SuccessMessage, name);
                sb.AppendLine(msg);
            }


            return sb.ToString();
        }

        public static string ImportClasses(StationsDbContext context, string jsonString)
        {
            var json = JsonConvert.DeserializeObject<List<SeatingClassDto>>(jsonString);

            var sb = new StringBuilder();

            foreach (var element in json)
            {
                string name = element.Name;
                string abbreviation = element.Abbreviation;

                bool isValid = IsValid(element);

                if (!isValid || context.SeatingClasses.Any(s => s.Name == name)
                    || context.SeatingClasses.Any(s => s.Abbreviation == abbreviation))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                var seatClass = new SeatingClass() { Name = name, Abbreviation = abbreviation };
                context.SeatingClasses.Add(seatClass);
                context.SaveChanges();

                var msg = string.Format(SuccessMessage, name);
                sb.AppendLine(msg);
            }


            return sb.ToString(); 
        }

        public static string ImportTrains(StationsDbContext context, string jsonString)
        {

            var json = JsonConvert.DeserializeObject<List<TrainDto>>(jsonString);

            var sb = new StringBuilder();

            foreach (var element in json)
            {

                string trainNumber = element.TrainNumber;
                string type = element.Type ?? "HighSpeed";

                var seats = element.Seats.ToList();
                var validSeats = new List<TrainSeat>();

                bool ifExist = false;

                bool isValid = IsValid(element);

                foreach (var seat in seats)
                {
                    string name = seat.Name;
                    string abbreviation = seat.Abbreviation;
                    int? quantity = seat.Quantity;

                    bool isValidSeat = IsValid(seat);

                    int? seatClassId = context.SeatingClasses
                        .SingleOrDefault(s => s.Name == name && s.Abbreviation == abbreviation)?.Id;

                    if (!context.SeatingClasses.Any(t => t.Id == seatClassId) || !isValidSeat)
                    {
                        ifExist = true;
                        break;
                    }

                    var trainSeat = new TrainSeat() { Quantity = (int)quantity, SeatingClassId = (int)seatClassId };
                    validSeats.Add(trainSeat);
                }

                if (ifExist || context.Trains.Any(t => t.TrainNumber == trainNumber) || !isValid)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                var train = new Train()
                {
                    TrainNumber = trainNumber,
                    TrainSeats = new List<TrainSeat>(validSeats),
                    Type = (TrainType?)Enum.GetNames(typeof(TrainType)).ToList().IndexOf(type)
                };

                context.Trains.Add(train);
                context.SaveChanges();

                var msg = string.Format(SuccessMessage, trainNumber);
                sb.AppendLine(msg);
            }


            return sb.ToString(); 
        }

        public static string ImportTrips(StationsDbContext context, string jsonString)
        {
            var json = JsonConvert.DeserializeObject<List<TripDto>>(jsonString);

            var sb = new StringBuilder();

            foreach (var element in json)
            {
                string train = element.Train;
                string originStation = element.OriginStation;
                string destinationStation = element.DestinationStation;
                var departureTime = element.DepartureTime;
                var arrivalTime = element.ArrivalTime;
                string status = element.Status ?? "OnTime";
                var timeDifference = element.TimeDifference;

                bool isValid = IsValid(element);

                if (!isValid || !context.Stations.Any(s => s.Name == originStation) || !context.Stations.Any(s => s.Name == destinationStation) || !context.Trains.Any(t => t.TrainNumber == train))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                int trainId = context.Trains.SingleOrDefault(t => t.TrainNumber == train).Id;
                int originId = context.Stations.SingleOrDefault(s => s.Name == originStation).Id;
                int destinationId = context.Stations.SingleOrDefault(s => s.Name == destinationStation).Id;

                var trip = new Trip()
                {
                    OriginStationId = originId,
                    DestinationStationId = destinationId,
                    DepartureTime = DateTime.ParseExact(departureTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture),
                    ArrivalTime = DateTime.ParseExact(arrivalTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture),
                    TrainId = trainId,
                    Status = (TripStatus)Enum.GetNames(typeof(TripStatus)).ToList().IndexOf(status),
                    TimeDifference = timeDifference
                };

                if (trip.DepartureTime > trip.ArrivalTime)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                context.Trips.Add(trip);
                context.SaveChanges();

                var msg = string.Format($"Trip from {originStation} to {destinationStation} imported.");
                sb.AppendLine(msg);
            }

            return sb.ToString();
        }

        public static string ImportCards(StationsDbContext context, string xmlString)
        {
            //var serializer = new XmlSerializer(typeof(CardDto), new XmlRootAttribute("Cards"));
            //var deserializedCards = (CardDto[])serializer.Deserialize(new MemoryStream(Encoding.UTF8.GetBytes(xmlString)));

            var xml = XDocument.Parse(xmlString);
            var root = xml.Root.Elements();


            var sb = new StringBuilder();

            foreach (var element in root)
            {
                var cardDto = new CardDto()
                {
                    Name = element.Element("Name")?.Value,
                    Age = (int?)int.Parse(element.Element("Age")?.Value) ?? null,
                    Type = element.Element("CardType")?.Value ?? "Normal"
                };

                bool isValid = IsValid(cardDto);

                if (!isValid)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                var card = new CustomerCard()
                {
                    Name = cardDto.Name,
                    Age = (int)cardDto.Age,
                    Type = (CardType)Enum.GetNames(typeof(CardType)).ToList().IndexOf(cardDto.Type)
                };


                context.Cards.Add(card);
                context.SaveChanges();

                var msg = string.Format(SuccessMessage, cardDto.Name);
                sb.AppendLine(msg);
            }

            return sb.ToString();
        }

        public static string ImportTickets(StationsDbContext context, string xmlString)
        {
            var xml = XDocument.Parse(xmlString);
            var root = xml.Root.Elements();


            var sb = new StringBuilder();

            foreach (var element in root)
            {
                var price = element.Attribute("price")?.Value ?? "-1";
                var seat = element.Attribute("seat")?.Value;

                var originStation = element.Element("Trip")?.Element("OriginStation")?.Value;
                var destinationStation = element.Element("Trip")?.Element("DestinationStation")?.Value;
                var departureTime = element.Element("Trip")?.Element("DepartureTime")?.Value;

                var cardName = element.Element("Card")?.Attribute("Name")?.Value;

                var ticketDto = new TicketDto()
                {
                    Price = decimal.Parse(price),
                    Seat = seat,
                    OriginStation = originStation,
                    DestinationStation = destinationStation,
                    DepartureTime = departureTime,
                    CardName = cardName
                };

                bool isValid = IsValid(ticketDto);

                if (!isValid)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                var departure = DateTime.ParseExact(departureTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);

                var trip = context.Trips
                    .Include(t => t.Train)
                    .ThenInclude(tr => tr.TrainSeats)
                    .SingleOrDefault(t => t.OriginStation.Name == ticketDto.OriginStation && t.DestinationStation.Name == ticketDto.DestinationStation && t.DepartureTime == departure);


                if (trip == null)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                if (ticketDto.CardName != null)
                {
                    var card = context.Cards.Any(c => c.Name == ticketDto.CardName);

                    if (!card)
                    {
                        sb.AppendLine(FailureMessage);
                        continue;
                    }
                }

                 
                string seatClass = string.Join("",ticketDto.Seat.Take(2));
                string seatNumber = string.Join("", ticketDto.Seat.Skip(2));

                var className = trip.Train.TrainSeats.SingleOrDefault(t => t.SeatingClass.Abbreviation == seatClass && int.Parse(seatNumber) <= t.Quantity);

                if (className == null || int.Parse(seatNumber) <= 0)
                {
                    sb.AppendLine(FailureMessage);
                    continue;

                }

                var ticket = new Ticket()
                {
                    Price = ticketDto.Price,
                    SeatingPlace = ticketDto.Seat,
                    TripId = trip.Id,
                    CustomerCardId = context.Cards.FirstOrDefault(c => c.Name == ticketDto.CardName)?.Id
                };

                context.Tickets.Add(ticket);
                context.SaveChanges();

                var msg = string.Format($"Ticket from {ticketDto.OriginStation} to {ticketDto.DestinationStation} departing at {departure.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture)} imported.");
                sb.AppendLine(msg);
            }

            return sb.ToString();
        }

        private static bool IsValid(object element)
        {
            var validationContext = new ValidationContext(element);
            var validationResults = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(element, validationContext, validationResults, true);

            return isValid;
        }
    }
}