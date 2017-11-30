using System;
using System.Linq;
using AutoMapper;
using Employees.App.Models;
using Employees.Data;

namespace Employees.App.Commands
{
    public class EmployeeInfoCommand
    {
        public string Execute(string[] data)
        {
            if (data.Length != 1)
            {
                throw new InvalidOperationException("Ivalid parameters!");
            }

            int employeeId = int.Parse(data[0]);
            string result = string.Empty;

            using (var db = new EmployeesContext())
            {
                var employee = db.Employees.Find(employeeId);

                if (employee == null)
                {
                    throw new ArgumentException("Invalid Employee");
                }

                var employeeDto = Mapper.Map<EmployeeDto>(employee);

                result = $"ID: {employeeDto.Id} - {employeeDto.FirstName} {employeeDto.LastName} - ${employeeDto.Salary:f2}";
            }
            return result;
        }
    }
}
