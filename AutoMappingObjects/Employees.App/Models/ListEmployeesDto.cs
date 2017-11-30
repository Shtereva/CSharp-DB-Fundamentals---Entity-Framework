namespace Employees.App.Models
{
    public class ListEmployeesDto
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public decimal Salary { get; set; }

        public ManagerDto Manager { get; set; }
    }
}
