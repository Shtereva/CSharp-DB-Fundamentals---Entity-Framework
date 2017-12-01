using System;
using System.Linq;
using System.Text;
using AutoMapper.QueryableExtensions;
using Employees.App.Models;
using Employees.Data;
using Employees.Services;

namespace Employees.App.Commands
{
    public class ManagerInfoCommand
    {
        private readonly EmployeeService employeeService;

        public ManagerInfoCommand(EmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        public string Execute(string[] data)
        {
            if (data.Length != 1)
            {
                throw new InvalidOperationException("Ivalid parameters!");
            }

            int managerId = int.Parse(data[0]);
            var sb = new StringBuilder();

            var manager = employeeService.ById<ManagerDto>(managerId);

            sb.AppendLine($"{manager.FirstName} {manager.LastName} | Employees: {manager.ManagedEmployeesCount}");
            sb.Append($"{string.Join(Environment.NewLine, manager.ManagedEmployees.Select(e => $"  - {e.FirstName} {e.LastName} - ${e.Salary:f2}").ToList())}");

            return sb.ToString();
        }
    }
}
