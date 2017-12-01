using System;
using System.Globalization;
using Employees.Data;
using Employees.Services;

namespace Employees.App.Commands
{
    public class SetBirthdayCommand
    {
        private readonly EmployeeService employeeService;

        public SetBirthdayCommand(EmployeeService employeeService)
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
            var birthday = DateTime.ParseExact(data[1], "dd-MM-yyyy", CultureInfo.InvariantCulture);

            employeeService.SetBirthday(employeeId, birthday);

            return $"Employee's birthday set to {birthday.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)}";
        }
    }
}
