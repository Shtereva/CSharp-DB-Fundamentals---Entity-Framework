using System;

namespace Employees.App
{
    public class Engine
    {
        private readonly CommandDispatcher commandDispatcher;
        private readonly IServiceProvider serviceProvider;
        public Engine(CommandDispatcher commandDispatcher, IServiceProvider serviceProvider)
        {
            this.commandDispatcher = commandDispatcher;
            this.serviceProvider = serviceProvider;
        }
        public void Run()
        {
            string input = string.Empty;

            while ((input = Console.ReadLine().Trim()) != "Exit")
            {
                try
                {
                    var data = input.Split();

                    string result = commandDispatcher.DispatchCommand(data, serviceProvider);
                    Console.WriteLine(result);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

    }
}
