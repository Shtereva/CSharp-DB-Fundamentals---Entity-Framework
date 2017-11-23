using System;
using System.Linq;
using Forum.Services.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace Forum.App
{
    public class Engine
    {
        private readonly IServiceProvider serviceProvider;
        public Engine(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }
        public void Run()
        {
            var databaseInitializerService = serviceProvider.GetService<IDatabaseInitializerService>();
            databaseInitializerService.InitializeDatabase();

            while (true)
            {
                Console.Write("Enter command: ");
                var input = Console.ReadLine();
                var commandTokens = input.Split();

                string commandName = commandTokens.First();

                var commandArg = commandTokens.Skip(1).ToArray();

                try
                {
                    var command = CommandParser.ParseCommand(serviceProvider, commandName);

                    var result = command.Execute(commandArg);

                    Console.WriteLine(result);
                }
                catch (InvalidOperationException e)
                {   
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
