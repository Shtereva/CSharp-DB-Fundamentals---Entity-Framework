using System;
using System.Linq;
using BusTicketSystem.Data;
using BusTicketSystem.Models;

namespace BusTicketSystem.Client.Core.Commands
{
    public class PublishReviewCommand
    {
        public string Execute(string[] data)
        {
            if (data.Length < 4)
            {
                throw new InvalidOperationException("Invalid parameters!");
            }

            int customerId = int.Parse(data[0]);
            double grade = double.Parse(data[1]);
            string busCompany = data[2];
            string content = string.Join(" ", data.Skip(3).ToArray());

            string result = string.Empty;

            using (var db = new BusTicketSystemContext())
            {
                if (!db.Customers.Any(c => c.Id == customerId) || !db.BusCompanies.Any(b => b.Name == busCompany))
                {
                    throw new ArgumentException("Invalid Custumer or Bus Company!");
                }

                if (grade < 1 || grade > 10)
                {
                    throw new ArgumentException("Grade can be between 1 and 10");
                }

                var review = new Review()
                {
                    BusCompanyId = db.BusCompanies.SingleOrDefault(b => b.Name == busCompany).Id,
                    Content = content,
                    CustomerId = customerId,
                    Grade = grade
                };

                db.Customers.Find(customerId).Reviews.Add(review);
                db.SaveChanges();

                string customerFullName = db.Customers.Find(customerId).FirstName + " " +
                                       db.Customers.Find(customerId).LastName;

                result = $"{customerFullName} review was succesfully published";
            }

            return result;
        }
    }
}
