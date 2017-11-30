using System;
using System.Linq;
using System.Text;
using AutoMapper.QueryableExtensions;
using Employees.App.Models;
using Employees.Data;

namespace Employees.App.Commands
{
    public class ManagerInfoCommand
    {
        public string Execute(string[] data)
        {
            if (data.Length != 1)
            {
                throw new InvalidOperationException("Ivalid parameters!");
            }

            int managerId = int.Parse(data[0]);
            var sb = new StringBuilder();

            using (var db = new EmployeesContext())
            {
                var manager = db.Employees
                    .Where(m => m.Id == managerId)
                    .ProjectTo<ManagerDto>()
                    .SingleOrDefault();


                sb.AppendLine($"{manager.FirstName} {manager.LastName} | Employees: {manager.ManagedEmployeesCount}");
                sb.Append($"   {string.Join(Environment.NewLine, manager.ManagedEmployees.Select(e => $"- {e.FirstName} {e.LastName} - ${e.Salary:f2}").ToList())}");
            }

            return sb.ToString();
        }
    }
}
