using System;
using System.Linq;
using System.Reflection;
using Forum.App.Commands.Contracts;

namespace Forum.App
{
    public class CommandParser
    {
        public static ICommand ParseCommand(IServiceProvider serviceProvider, string commandName)
        {
            var assebly = Assembly.GetExecutingAssembly();

            var commandTypes = assebly.GetTypes()
                .Where(x => x.GetInterfaces().Contains(typeof(ICommand)))
                .ToArray();

            var commandType = commandTypes.SingleOrDefault(t => t.Name == $"{commandName}Command");

            if (commandType == null)
            {
                throw new InvalidOperationException("Invalid command");
            }

            var constructor = commandType.GetConstructors().First();

            var constructorParameters = constructor.GetParameters()
                .Select(pi => pi.ParameterType)
                .ToArray();

            var services = constructorParameters
                .Select(serviceProvider.GetService)
                .ToArray();

            var command = (ICommand)constructor.Invoke(services);

            return command;
        }
    }
}
