using System;
using System.Linq;
using AutoMapper;
using Employees.App.Models;
using Employees.Data;
using Employees.Services;

namespace Employees.App.Commands
{
    public class EmployeeInfoCommand
    {
        private readonly EmployeeService employeeService;

        public EmployeeInfoCommand(EmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        public string Execute(string[] data)
        {
            if (data.Length != 1)
            {
                throw new InvalidOperationException("Ivalid parameters!");
            }

            int employeeId = int.Parse(data[0]);

            var employee = employeeService.ById<EmployeeDto>(employeeId);

            var result = $"ID: {employee.Id} - {employee.FirstName} {employee.LastName} - ${employee.Salary:f2}";

            return result;
        }
    }
}
