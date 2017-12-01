using System;
using System.Globalization;
using System.Linq;
using Employees.Data;
using Employees.Services;

namespace Employees.App.Commands
{
    public class SetAddressCommand
    {
        private readonly EmployeeService employeeService;

        public SetAddressCommand(EmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        public string Execute(string[] data)
        {
            if (data.Length < 2)
            {
                throw new InvalidOperationException("Ivalid parameters!");
            }

            int employeeId = int.Parse(data[0]);
            string address = string.Join(" ", data.Skip(1));

            employeeService.SetAddress(employeeId, address);

            return $"Employee's address set to {address}";
        }
    }
}
