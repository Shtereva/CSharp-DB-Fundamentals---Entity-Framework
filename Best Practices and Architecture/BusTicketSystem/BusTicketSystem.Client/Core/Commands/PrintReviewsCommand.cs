using System;
using System.Globalization;
using System.Linq;
using BusTicketSystem.Data;
using BusTicketSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace BusTicketSystem.Client.Core.Commands
{
    public class PrintReviewsCommand
    {
        public string Execute(string[] data)
        {
            if (data.Length != 1)
            {
                throw new InvalidOperationException("Invalid parameters!");
            }

            int busCompanyId = int.Parse(data[0]);
            string printText = string.Empty;

            using (var db = new BusTicketSystemContext())
            {
                if (!db.BusCompanies.Any(c => c.Id == busCompanyId))
                {
                    throw new ArgumentException("There is no such Bus Company!");
                }

                var result = db.Reviews.Where(r => r.BusCompanyId == busCompanyId)
                    .Select(r => new
                    {
                        r.Id,
                        r.Grade,
                        PublishingDateTime = r.PublishingDateTime,
                        r.Content,
                        FullName = r.Customer.FirstName + " " + r.Customer.LastName
                    }).ToArray();

                var formatResult = result
                    .Select(r => $"{r.Id} {r.Grade} {r.PublishingDateTime}{Environment.NewLine}{r.FullName}{Environment.NewLine}{r.Content}")
                    .ToArray();

                printText = string.Join(Environment.NewLine, formatResult);
            }

            return printText;
        }
    }
}
