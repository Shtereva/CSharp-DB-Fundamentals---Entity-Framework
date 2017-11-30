using System;

namespace Employees.App
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
            string input = string.Empty;

            while ((input = Console.ReadLine().Trim()) != "Exit")
            {
                //try
                //{
                    var data = input.Split();

                    string result = commandDispatcher.DispatchCommand(data);
                    Console.WriteLine(result);
                //}
                //catch (Exception e)
                //{
                //    Console.WriteLine(e.Message);
                //}
            }
        }

    }
}
