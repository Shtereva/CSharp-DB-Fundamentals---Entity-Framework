using System;
using System.Linq;
using System.Text;
using AutoMapper.QueryableExtensions;
using Employees.App.Models;
using Employees.Data;

namespace Employees.App.Commands
{
    public class ListEmployeesOlderThanCommand
    {
        public string Execute(string[] data)
        {
            if (data.Length != 1)
            {
                throw new InvalidOperationException("Ivalid parameters!");
            }

            int age = int.Parse(data[0]);
            var sb = new StringBuilder();

            using (var db = new EmployeesContext())
            {
                var employees = db.Employees
                    .Where(e => Math.Abs(DateTime.Now.Year - e.BirthDay.Value.Year) > age)
                    .ProjectTo<ListEmployeesDto>()
                    .OrderByDescending(e => e.Salary)
                    .ToList();

                var formatResult = employees
                    .Select(e =>
                        e.Manager == null
                            ? $"{e.FirstName} {e.LastName} - ${e.Salary:f2} - Manager: [no manager]"
                            : $"{e.FirstName} {e.LastName} - ${e.Salary:f2} - Manager: {e.Manager.LastName}")
                    .ToList();

                sb.Append(string.Join(Environment.NewLine, formatResult));

            }

            return sb.ToString();
        }
    }
}
