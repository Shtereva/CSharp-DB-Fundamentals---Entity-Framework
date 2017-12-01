using System;
using System.Linq;
using Employees.Data;
using Employees.Services;

namespace Employees.App.Commands
{
    public class SetManagerCommand
    {
        private readonly EmployeeService employeeService;

        public SetManagerCommand(EmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }
        public string Execute(string[] data)
        {
            if (data.Length != 2)
            {
                throw new InvalidOperationException("Ivalid parameters!");
            }

            int employeeId = int.Parse(data[0]);
            int managerId = int.Parse(data[1]);

            employeeService.SetManager(employeeId, managerId);

            return "Success";
        }
    }
}
