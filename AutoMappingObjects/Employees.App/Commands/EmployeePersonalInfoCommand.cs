using System;
using System.Globalization;
using System.Text;
using AutoMapper;
using Employees.App.Models;
using Employees.Data;
using Employees.Services;

namespace Employees.App.Commands
{
    public class EmployeePersonalInfoCommand
    {
        private readonly EmployeeService employeeService;

        public EmployeePersonalInfoCommand(EmployeeService employeeService)
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
            var result = new StringBuilder();

            var employee = employeeService.ById<EmployeeFullInfoDto>(employeeId);

            result.AppendLine($"ID: {employee.Id} - {employee.FirstName} {employee.LastName} - ${employee.Salary:f2}");
            result.AppendLine($"Birthday: {employee.BirthDay:dd-MM-yyyy}");
            result.Append($"Address: {employee.Address}");

            return result.ToString();
        }
    }
}
