using AutoMapper;
using Employees.Data;

namespace Employees.App
{
    class StartUp
    {
        static void Main(string[] args)
        {
            //DbInitializer();

            Mapper.Initialize(cfg => cfg.AddProfile<EmployeeProfile>());

            var commandDispatcher = new CommandDispatcher();
            var engine = new Engine(commandDispatcher);
            engine.Run();
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
