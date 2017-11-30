using System.Collections;
using System.Collections.Generic;
using Employees.Models;

namespace Employees.App.Models
{
    public class ManagerDto
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public IEnumerable<Employee> ManagedEmployees { get; set; }

        public int ManagedEmployeesCount { get; set; }
    }
}
