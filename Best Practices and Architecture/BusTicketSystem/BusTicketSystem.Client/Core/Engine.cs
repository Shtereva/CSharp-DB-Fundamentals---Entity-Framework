using System;

namespace BusTicketSystem.Client.Core
{
    public class Engine
    {
        private readonly CommandDispatcher commandDispatcher;

        public Engine(CommandDispatcher commandDispatcher)
        {
            this.commandDispatcher = commandDispatcher;
        }

        public void Run()
        {
            string command;

            while ((command = Console.ReadLine().Trim()) != " Exit")
            {
                try
                {
                    var data = command.Split();
                    string result = commandDispatcher.DispatchCommand(data);
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
