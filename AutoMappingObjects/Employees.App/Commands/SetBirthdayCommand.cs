using System;
using System.Globalization;
using Employees.Data;

namespace Employees.App.Commands
{
    public class SetBirthdayCommand
    {
        public string Execute(string[] data)
        {
            if (data.Length != 2)
            {
                throw new InvalidOperationException("Ivalid parameters!");
            }

            int employeeId = int.Parse(data[0]);
            var birthday = DateTime.ParseExact(data[1], "dd-MM-yyyy", CultureInfo.InvariantCulture);

            using (var db = new EmployeesContext())
            {
                var employee = db.Employees.Find(employeeId);

                if (employee == null)
                {
                    throw new ArgumentException("Invalid Employee");
                }

                employee.BirthDay = birthday;

                db.SaveChanges();
            }

            return $"Employee's birthday set to {birthday.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)}";
        }
    }
}
