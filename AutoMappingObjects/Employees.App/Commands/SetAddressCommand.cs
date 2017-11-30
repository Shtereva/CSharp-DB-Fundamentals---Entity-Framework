using System;
using System.Globalization;
using System.Linq;
using Employees.Data;

namespace Employees.App.Commands
{
    public class SetAddressCommand
    {
        public string Execute(string[] data)
        {
            if (data.Length < 2)
            {
                throw new InvalidOperationException("Ivalid parameters!");
            }

            int employeeId = int.Parse(data[0]);
            string address = string.Join(" ", data.Skip(1));

            using (var db = new EmployeesContext())
            {
                var employee = db.Employees.Find(employeeId);

                if (employee == null)
                {
                    throw new ArgumentException("Invalid Employee");
                }

                employee.Address = address;

                db.SaveChanges();
            }

            return $"Employee's address set to {address}";
        }
    }
}
