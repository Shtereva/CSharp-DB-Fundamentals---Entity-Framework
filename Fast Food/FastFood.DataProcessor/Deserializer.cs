using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;
using FastFood.Data;
using FastFood.DataProcessor.Dto.Import;
using FastFood.Models;
using FastFood.Models.Enums;
using Newtonsoft.Json;

namespace FastFood.DataProcessor
{
	public static class Deserializer
	{
		private const string FailureMessage = "Invalid data format.";
		private const string SuccessMessage = "Record {0} successfully imported.";

		public static string ImportEmployees(FastFoodDbContext context, string jsonString)
		{
		    var employees = JsonConvert.DeserializeObject<EmployeeDto[]>(jsonString);

            var sb = new StringBuilder();

		    foreach (var employee in employees)
		    {
		        bool isValid = IsValid(employee);

                if (!isValid)
		        {
		            sb.AppendLine(FailureMessage);
                    continue;
		        }

                if (!context.Positions.Any(p => p.Name == employee.Position))
                {
                    var position = new Position()
                    {
                        Name = employee.Position
                    };

                    context.Positions.Add(position);
                    context.SaveChanges();
                }

                var empl = new Employee()
                {
                    Name = employee.Name,
                    Age = employee.Age,
                    PositionId = context.Positions.SingleOrDefault(p => p.Name == employee.Position).Id
                };

		        context.Employees.Add(empl);
		        context.SaveChanges();

		        sb.AppendLine(string.Format(SuccessMessage, employee.Name));
		    }

            return sb.ToString();
		}

		public static string ImportItems(FastFoodDbContext context, string jsonString)
		{
		    var items = JsonConvert.DeserializeObject<ItemDto[]>(jsonString);

		    var sb = new StringBuilder();

		    foreach (var item in items)
		    {
		        bool isValid = IsValid(item);

		        if (!isValid || context.Items.Any(i => i.Name == item.Name))
		        {
		            sb.AppendLine(FailureMessage);
		            continue;
		        }

		        if (!context.Categories.Any(c => c.Name == item.Category))
		        {
		            var category = new Category()
		            {
		                Name = item.Category
		            };

		            context.Categories.Add(category);
		            context.SaveChanges();
		        }

		        var itm = new Item()
		        {
		            Name = item.Name,
                    Price = item.Price,
                    CategoryId = context.Categories.SingleOrDefault(c => c.Name == item.Category).Id
		        };

		        context.Items.Add(itm);
		        context.SaveChanges();

		        sb.AppendLine(string.Format(SuccessMessage, item.Name));
		    }

		    return sb.ToString();
        }

		public static string ImportOrders(FastFoodDbContext context, string xmlString)
		{
		    var sb = new StringBuilder();

		    var xml = XDocument.Parse(xmlString);
		    var root = xml.Root.Elements();

		    foreach (var element in root)
		    {
		        string customer = element.Element("Customer")?.Value;
		        string employee = element.Element("Employee")?.Value;
		        var dateTime = DateTime.ParseExact(element.Element("DateTime")?.Value, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
		        string type = element.Element("Type")?.Value.ToString();

                var orderDto = new OrderDto()
                {
                    Customer = customer,
                    Employee = employee,
                    DateTime = dateTime.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture),
                    Type = type
                };

		        if (!IsValid(orderDto) || !context.Employees.Any(e => e.Name == employee))
		        {
		            sb.AppendLine(FailureMessage);
		            continue;
                }

		        var itemsElements = element.Element("Items")?.Elements().ToList();
                var items = new List<OrderItem>();

		        foreach (var item in itemsElements)
		        {
		            var quantity = item.Element("Quantity")?.Value ?? "0";
		            string name = item.Element("Name")?.Value;

                    var orderItem = new OrderItemDto()
		            {
		                Name = name,
                        Quantity = int.Parse(quantity)
                    };

		            if (!IsValid(orderItem) || !context.Items.Any(i => i.Name == name))
		            {
		                sb.AppendLine(FailureMessage);
                        continue;
		            }

                    var final = new OrderItem()
                    {
                        ItemId = context.Items.SingleOrDefault(i => i.Name == name).Id,
                        Quantity = int.Parse(quantity)
                    };

                    items.Add(final);
		        }

                orderDto.Items.AddRange(items);

                var order = new Order()
                {
                    Customer = orderDto.Customer,
                    DateTime = dateTime,
                    Type = Enum.Parse<OrderType>(orderDto.Type),
                    EmployeeId = context.Employees.SingleOrDefault(e => e.Name == orderDto.Employee).Id,
                    OrderItems = new List<OrderItem>(orderDto.Items)
                };

		        context.Orders.Add(order);
		        context.SaveChanges();
		        sb.AppendLine($"Order for {customer} on {dateTime.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture)} added");
		    }

		    return sb.ToString();
		}

	    private static bool IsValid(object obj)
	    {
	        var validationContext = new ValidationContext(obj);
	        var validationResults = new List<ValidationResult>();

	        var isValid = Validator.TryValidateObject(obj, validationContext, validationResults, true);
	        return isValid;
	    }
    }
}