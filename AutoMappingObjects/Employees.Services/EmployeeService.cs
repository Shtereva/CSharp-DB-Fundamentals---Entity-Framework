using System;
using System.Linq;
using Employees.Data;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Employees.Models;


namespace Employees.Services
{
    public class EmployeeService
    {
        private readonly EmployeesContext context;

        public EmployeeService(EmployeesContext context)
        {
            this.context = context;
        }

        public void Create(string firstName, string lastName, decimal salary)
        {
            var employee = new Employee()
            {
                FirstName = firstName,
                LastName = lastName,
                Salary = salary
            };

            context.Employees.Add(employee);
            context.SaveChanges();
        }

        public TModel ById<TModel>(int employeeId)
        {
            var employee = context.Employees
                .Where(e => e.Id == employeeId)
                .ProjectTo<TModel>()
                .SingleOrDefault();

            if (employee == null)
            {
                throw new ArgumentException("Invalid Employee");
            }

            return employee;
        }

        public IQueryable<TModel> ListEmployeesOlderThan<TModel>(int age)
        {
            var employees = context.Employees
                .Where(e => Math.Abs(DateTime.Now.Year - e.BirthDay.Value.Year) > age)
                .ProjectTo<TModel>();

            return employees;
        }

        public void SetAddress(int employeeId, string address)
        {
            var employee = context.Employees.Find(employeeId);

            if (employee == null)
            {
                throw new ArgumentException("Invalid Employee");
            }

            employee.Address = address;

            context.SaveChanges();
        }

        public void SetBirthday(int employeeId, DateTime birthday)
        {
            var employee = context.Employees.Find(employeeId);

            if (employee == null)
            {
                throw new ArgumentException("Invalid Employee");
            }

            employee.BirthDay = birthday;

            context.SaveChanges();
        }

        public void SetManager(int employeeId, int managerId)
        {
            var employee = context.Employees.Find(employeeId);

            if (employee == null)
            {
                throw new ArgumentException("Invalid Employee");
            }

            employee.ManagerId = managerId;

            context.SaveChanges();
        }
    }
}
