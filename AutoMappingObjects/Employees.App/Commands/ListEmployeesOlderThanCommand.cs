using System;
using System.Linq;
using System.Text;
using AutoMapper.QueryableExtensions;
using Employees.App.Models;
using Employees.Data;
using Employees.Services;

namespace Employees.App.Commands
{
    public class ListEmployeesOlderThanCommand
    {
        private readonly EmployeeService employeeService;

        public ListEmployeesOlderThanCommand(EmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        public string Execute(string[] data)
        {
            if (data.Length != 1)
            {
                throw new InvalidOperationException("Ivalid parameters!");
            }

            int age = int.Parse(data[0]);
            var sb = new StringBuilder();

            var employees = employeeService.ListEmployeesOlderThan<ListEmployeesDto>(age).ToList();

            var formatResult = employees
                .OrderByDescending(e => e.Salary)
                .Select(e =>
                    e.Manager == null
                        ? $"{e.FirstName} {e.LastName} - ${e.Salary:f2} - Manager: [no manager]"
                        : $"{e.FirstName} {e.LastName} - ${e.Salary:f2} - Manager: {e.Manager.LastName}")
                .ToList();

            sb.Append(string.Join(Environment.NewLine, formatResult));

            return sb.ToString();
        }
    }
}
