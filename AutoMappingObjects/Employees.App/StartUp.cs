using System;
using AutoMapper;
using Employees.Data;
using Employees.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Employees.App
{
    class StartUp
    {
        static void Main(string[] args)
        {
            //DbInitializer();

            var serviceProvider = ConfigureServices();

            var commandDispatcher = new CommandDispatcher();
            var engine = new Engine(commandDispatcher, serviceProvider);
            engine.Run();
        }

        private static IServiceProvider ConfigureServices()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddDbContext<EmployeesContext>(opt => opt.UseSqlServer(Configuration.ConnectionString));
            serviceCollection.AddTransient<EmployeeService>();

            serviceCollection.AddAutoMapper(cfg => cfg.AddProfile<EmployeeProfile>());

            var serviceProvider = serviceCollection.BuildServiceProvider();

            return serviceProvider;
        }

        private static void DbInitializer()
        {
            using (var db = new EmployeesContext())
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
            }
        }
    }
}
