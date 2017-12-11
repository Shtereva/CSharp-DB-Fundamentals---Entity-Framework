using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using FastFood.Data;
using FastFood.Models.Enums;
using Newtonsoft.Json;

namespace FastFood.DataProcessor
{
    public class Serializer
    {
        public static string ExportOrdersByEmployee(FastFoodDbContext context, string employeeName, string orderType)
        {
            var employees = context.Employees
                .Where(e => e.Name == employeeName && e.Orders.Any(o => o.Type == Enum.Parse<OrderType>(orderType)))
                .Select(e => new
                {
                    Name = e.Name,
                    Orders = e.Orders.Select(o => new
                    {
                        Customer = o.Customer,
                        Items = o.OrderItems.Select(oi => new
                        {
                            Name = oi.Item.Name,
                            Price = oi.Item.Price,
                            Quantity = oi.Quantity,
                        }).ToArray(),
                        TotalPrice = o.OrderItems.Select(oi => oi.Item.Price * oi.Quantity).Sum()
                    })
                    .OrderByDescending(o => o.TotalPrice)
                    .ThenByDescending(o => o.Items.Length)
                    .ToArray(),
                }).ToArray();


            var result = employees
                .Select(e => new
                {
                    Name = e.Name,
                    Orders = e.Orders,
                    TotalMade = e.Orders.Select(o => o.TotalPrice).Sum()
                })
                .SingleOrDefault();



            string json = JsonConvert.SerializeObject(result, Formatting.Indented);

            return json;
        }

        public static string ExportCategoryStatistics(FastFoodDbContext context, string categoriesString)
        {
            var categoriesToArray = categoriesString.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            var categories = context.Categories
                .Where(c => categoriesToArray.Contains(c.Name))
                .Select(c => new
                {
                    Name = c.Name,
                    MostPopularItem = c.Items
                    .Select(i => new
                    {
                        Name = i.Name,
                        TotalMade = i.OrderItems.Select(o => o.Quantity * o.Item.Price).Sum(),
                        TimesSold = i.OrderItems.Sum(o => o.Quantity)
                    }).OrderByDescending(x => x.TimesSold).First()
                })
                .OrderByDescending(c => c.MostPopularItem.TotalMade)
                .ThenByDescending(c => c.MostPopularItem.TimesSold)
                .ToArray();


            var xDox = new XDocument();
            var xElements = new List<XElement>();

            foreach (var category in categories)
            {
                xElements.Add(new XElement("Category",
                                new XElement("Name", category.Name),
                                new XElement("MostPopularItem", 
                                    new XElement("Name", category.MostPopularItem.Name),
                                    new XElement("TotalMade", category.MostPopularItem.TotalMade),
                                    new XElement("TimesSold", category.MostPopularItem.TimesSold))));
            }

            xDox.Add(new XElement("Categories", xElements));

            return xDox.ToString();
        }
    }
}