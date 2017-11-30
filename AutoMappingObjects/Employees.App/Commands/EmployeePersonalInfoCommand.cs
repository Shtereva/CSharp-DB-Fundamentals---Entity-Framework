using System;
using System.Globalization;
using System.Text;
using AutoMapper;
using Employees.App.Models;
using Employees.Data;

namespace Employees.App.Commands
{
    public class EmployeePersonalInfoCommand
    {
        public string Execute(string[] data)
        {
            if (data.Length != 1)
            {
                throw new InvalidOperationException("Ivalid parameters!");
            }

            int employeeId = int.Parse(data[0]);
            var result = new StringBuilder();

            using (var db = new EmployeesContext())
            {
                var employee = db.Employees.Find(employeeId);

                if (employee == null)
                {
                    throw new ArgumentException("Invalid Employee");
                }

                var employeeDto = Mapper.Map<EmployeeFullInfoDto>(employee);

                result.AppendLine($"ID: {employeeDto.Id} - {employeeDto.FirstName} {employeeDto.LastName} - ${employeeDto.Salary:f2}");
                result.AppendLine($"Birthday: {employeeDto.BirthDay:dd-MM-yyyy}");
                result.Append($"Address: {employeeDto.Address}");
            }
            return result.ToString();
        }
    }
}
