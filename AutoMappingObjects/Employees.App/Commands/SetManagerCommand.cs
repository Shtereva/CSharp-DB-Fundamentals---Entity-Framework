using System;
using System.Linq;
using Employees.Data;

namespace Employees.App.Commands
{
    public class SetManagerCommand
    {
        public string Execute(string[] data)
        {
            if (data.Length != 2)
            {
                throw new InvalidOperationException("Ivalid parameters!");
            }

            int employeeId = int.Parse(data[0]);
            int managerId = int.Parse(data[1]);

            using (var db = new EmployeesContext())
            {
                var employee = db.Employees.Find(employeeId);

                if (employee == null || !db.Employees.Any(e => e.Id == managerId))
                {
                    throw new ArgumentException("Invalid Employee");
                }

                employee.ManagerId = managerId;
                db.SaveChanges();
            }

            return "Success";
        }
    }
}
