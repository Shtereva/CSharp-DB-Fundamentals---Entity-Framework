using System;
using Employees.Data;
using Employees.Models;

namespace Employees.App.Commands
{
    public class AddEmployeeCommand
    {
        public string Execute(string[] data)
        {
            if (data.Length != 3)
            {
                throw new InvalidOperationException("Ivalid parameters!");
            }

            string firstName = data[0];
            string lastName = data[1];
            decimal salary = decimal.Parse(data[2]);

            using (var db = new EmployeesContext())
            {
                var employee = new Employee()
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Salary = salary
                };

                db.Employees.Add(employee);
                db.SaveChanges();
            }

            return $"Employee {firstName} {lastName} succsessfully added!";
        }
    }
}
