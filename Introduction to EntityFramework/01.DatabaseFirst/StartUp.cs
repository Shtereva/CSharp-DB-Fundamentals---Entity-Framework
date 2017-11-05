
using System;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using Microsoft.EntityFrameworkCore;
using P02_DatabaseFirst.Data;
using P02_DatabaseFirst.Data.Models;

namespace P02_DatabaseFirst
{
    class StartUp
    {
        static void Main(string[] args)
        {
            // 3. Employees Full Information
            // -----------------------------
            //using (var db = new SoftUniContext())
            //{
            //db.Employees.Select(e => new
            //{
            //    e.FirstName,
            //    e.LastName,
            //    e.MiddleName,
            //    e.JobTitle,
            //    e.Salary,
            //    e.EmployeeId
            //})
            //.OrderBy(e => e.EmployeeId)
            //.ToList()
            //.ForEach(e => Console.WriteLine($"{e.FirstName} {e.LastName} {e.MiddleName} {e.JobTitle} {e.Salary:f2}"));
            //}


            // 4. Employees with Salary Over 50 000
            // ------------------------------------
            //using (var db = new SoftUniContext())
            //{
            //    db.Employees
            //        .Select(e => new
            //        {
            //            e.FirstName,
            //            e.Salary
            //        })
            //        .Where(e => e.Salary > 50000)
            //        .OrderBy(e => e.FirstName)
            //        .ToList()
            //        .ForEach(e => Console.WriteLine($"{e.FirstName}"));
            //}		

            //5. Employees from Research and Development
            // -----------------------------------------
            //using (var db = new SoftUniContext())
            //{
            //    db.Employees
            //        .Include(e => e.Department)
            //        .Where(e => e.Department.Name == "Research and Development")
            //        .OrderBy(e => e.Salary)
            //        .ThenByDescending(e => e.FirstName)
            //        .Select(e => new
            //        {
            //            e.FirstName,
            //            e.LastName,
            //            e.Department.Name,
            //            e.Salary
            //        })
            //        .ToList()
            //        .ForEach(e => Console.WriteLine($"{e.FirstName} {e.LastName} from {e.Name} - ${e.Salary:f2}"));
            //}

            // 6. Adding a New Address and Updating Employee
            // ---------------------------------------------
            //using (var db = new SoftUniContext())
            //{
            //    var id = db.Employees.SingleOrDefault(e => e.LastName == "Nakov").EmployeeId;

            //    db.Employees.Find(id).Address = new Address()
            //                                        {
            //                                            AddressText = "Vitoshka 15",
            //                                            TownId = 4
            //                                        };
            //    db.SaveChanges();

            //    db.Employees
            //        .Include(e => e.Address)
            //        .Select(e => new
            //        {
            //            e.AddressId,
            //            e.Address.AddressText
            //        })
            //        .OrderByDescending(e => e.AddressId)
            //        .Take(10)
            //        .ToList()
            //        .ForEach(e => Console.WriteLine(e.AddressText));
            //}		

            // 7. Employees and Projects
            // -------------------------
            //using (var db = new SoftUniContext())
            //{
            //    var employeesProjects = db.Employees
            //        .Include(e => e.EmployeesProjects)
            //        .ThenInclude(e => e.Project)
            //        .Where(e => e.EmployeesProjects.Any(p => p.Project.StartDate.Year >= 2001 && p.Project.StartDate.Year <= 2003))
            //        .Take(30)
            //        .ToList();

            //    foreach (var employee in employeesProjects)
            //    {
            //        var managerId = employee.ManagerId;
            //        var manager = db.Employees.Find(managerId);

            //        Console.WriteLine($"{employee.FirstName} {employee.LastName} – Manager: {manager.FirstName} {manager.LastName}");


            //        foreach (var project in employee.EmployeesProjects)
            //        {
            //            string format = "M/d/yyyy h:mm:ss tt";

            //            string startDate = project.Project.StartDate.ToString(format, CultureInfo.InvariantCulture);

            //            string endDate = project.Project.EndDate.ToString();

            //            if (String.IsNullOrWhiteSpace(endDate))
            //            {
            //                endDate = "not finished";
            //            }
            //            else
            //            {
            //                endDate = project.Project.EndDate.Value.ToString(format, CultureInfo.InvariantCulture);
            //            }

            //            Console.WriteLine($"--{project.Project.Name} - {startDate} - {endDate}");
            //        }
            //    }
            //}

            // 8. Addresses by Town
            // --------------------
            //using (var db = new SoftUniContext())
            //{
            //    db.Employees
            //        .Include(e => e.Address)
            //        .ThenInclude(e => e.Town)
            //        .GroupBy(e => new  {e.Address.AddressText, e.Address.Town.Name, e.Address.Employees.Count})
            //        .OrderByDescending(e => e.Key.Count)
            //        .ThenBy(e => e.Key.Name)
            //        .ThenBy(e => e.Key.AddressText)
            //        .Take(10)
            //        .ToList()
            //        .ForEach(e => Console.WriteLine($"{e.Key.AddressText}, {e.Key.Name} - {e.Key.Count} employees"));
            //}

            // 9. Employee 147
            // ---------------
            //using (var db= new SoftUniContext())
            //{
            //    var employee = db.Employees
            //        .Include(e => e.EmployeesProjects)
            //        .ThenInclude(e => e.Project)
            //        .SingleOrDefault(e => e.EmployeeId == 147);

            //    Console.WriteLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle}");
            //    employee.EmployeesProjects
            //        .OrderBy(p => p.Project.Name)
            //        .ToList()
            //        .ForEach(p => Console.WriteLine(p.Project.Name));
            //}

            // 10. Departments with More Than 5 Employees
            // ----------------------------------------
            //using (var db = new SoftUniContext())
            //{
            //    var departments = db.Departments
            //        .Include(d => d.Employees)
            //        .Where(d => d.Employees.Count > 5)
            //        .OrderBy(d => d.Employees.Count)
            //        .ThenBy(d => d.Name)
            //        .ToList();

            //    foreach (var department in departments)
            //    {
            //        var manager = db.Employees.Find(department.ManagerId);
            //        Console.WriteLine($"{department.Name} – {manager.FirstName} {manager.LastName}");

            //        department.Employees
            //            .OrderBy(e => e.FirstName)
            //            .ThenBy(e => e.LastName)
            //            .ToList()
            //            .ForEach(e => Console.WriteLine($"{e.FirstName} {e.LastName} - {e.JobTitle}"));

            //        Console.WriteLine(new string('-', 10));
            //    }
            //}

            // 11. Find Latest 10 Projects
                // -----------------------------
                //using (var db = new SoftUniContext())
                //{
                //    var projects = db.Projects
                //        .OrderByDescending(p => p.StartDate)
                //        .Take(10)
                //        .OrderBy(p => p.Name)
                //        .ToList();

                //    foreach (var project in projects)
                //    {
                //        Console.WriteLine(project.Name);
                //        Console.WriteLine(project.Description);
                //        Console.WriteLine(project.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture));
                //    }
                //}


                // 12. Increase Salaries
                // ---------------------
                //using (var db = new SoftUniContext())
                //{
                //    var employees = db.Employees
                //        .Include(e => e.Department)
                //        .Where(e => e.Department.Name == "Engineering" || e.Department.Name == "Tool Design" ||
                //                    e.Department.Name == "Marketing" ||
                //                    e.Department.Name == "Information Services")
                //        .OrderBy(e => e.FirstName)
                //        .ThenBy(e => e.LastName)
                //        .ToList();
                //    for (int i = 0; i < employees.Count; i++)
                //    {
                //        employees[i].Salary *= 1.12M;
                //    }

                //    employees
                //    .ForEach(e => Console.WriteLine($"{e.FirstName} {e.LastName} (${(e.Salary):f2})"));

                //    db.SaveChanges();
                //}

                // 13. Find Employees by First Name Starting With Sa
                // --------------------------------------------------
                //using (var db = new SoftUniContext())
                //{
                //    db.Employees
                //        .Where(e => e.FirstName[0] == 'S' && e.FirstName[1] == 'a')
                //        .Select(e => new
                //        {
                //            e.FirstName,
                //            e.LastName,
                //            e.JobTitle,
                //            e.Salary
                //        })
                //        .OrderBy(e => e.FirstName)
                //        .ThenBy(e => e.LastName)
                //        .ToList()
                //        .ForEach(e => Console.WriteLine($"{e.FirstName} {e.LastName} - {e.JobTitle} - (${e.Salary:f2})"));
                //}

                // 14. Delete Project by Id
                // --------------------------
                //using (var db = new SoftUniContext())
                //{
                //var project = db.Projects.Find(2);
                //var removed = db.EmployeesProjects.Where(e => e.ProjectId == 2).ToList();

                //db.EmployeesProjects.RemoveRange(removed);
                //db.SaveChanges();

                //db.Projects.Remove(project);
                //db.SaveChanges();
                //var em = db.EmployeesProjects.Where(e => e.ProjectId == 2).ToList();

                // db.Projects.Take(10).ToList().ForEach(p => Console.WriteLine(p.Name));
                //}

                // 15. Remove Towns
                // ----------------
                //using (var db = new SoftUniContext())
                //{
                //    var town = db.Towns.SingleOrDefault(t => t.Name == "Seattle");

                //    var addresses = db.Addresses
                //        .Where(a => a.TownId == town.TownId)
                //        .ToList();

                //    foreach (var address in addresses)
                //    {
                //        while (db.Employees.Any(e => e.AddressId == address.AddressId))
                //        {
                //            db.Employees.FirstOrDefault(e => e.AddressId == address.AddressId).AddressId = null;
                //            db.SaveChanges();

                //        }
                //    }

                //    db.Addresses.RemoveRange(addresses);
                //    db.SaveChanges();

                //    db.Towns.Remove(town);
                //    db.SaveChanges();

                //    Console.WriteLine($"{addresses.Count} addresses in Seattle were deleted");
                //}			
            }
    }
}

