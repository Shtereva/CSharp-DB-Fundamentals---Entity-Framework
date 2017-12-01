using System;
using Employees.App.Models;
using Employees.Data;
using Employees.Models;
using Employees.Services;

namespace Employees.App.Commands
{
    public class AddEmployeeCommand
    {
        private readonly EmployeeService employeeService;

        public AddEmployeeCommand(EmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        public string Execute(string[] data)
        {
            if (data.Length != 3)
            {
                throw new InvalidOperationException("Ivalid parameters!");
            }

            string firstName = data[0];
            string lastName = data[1];
            decimal salary = decimal.Parse(data[2]);

            employeeService.Create(firstName, lastName, salary);

            return $"Employee {firstName} {lastName} succsessfully added!";
        }
    }
}
