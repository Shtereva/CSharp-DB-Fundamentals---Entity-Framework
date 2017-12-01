using System;
using System.Linq;
using Employees.App.Commands;
using Employees.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Employees.App
{
    public class CommandDispatcher
    {
        public string DispatchCommand(string[] commandParams, IServiceProvider serviceProvider)
        {
            string commandName = commandParams[0];

            var commandArg = commandParams.Skip(1).ToArray();

            string result = string.Empty;

            switch (commandName)
            {
                case "AddEmployee":
                    var addEmployee = new AddEmployeeCommand(serviceProvider.GetService<EmployeeService>());
                    result = addEmployee.Execute(commandArg);
                    break;
                case "SetBirthday":
                    var setBirthday = new SetBirthdayCommand(serviceProvider.GetService<EmployeeService>());
                    result = setBirthday.Execute(commandArg);
                    break;
                case "SetAddress":
                    var setAddress = new SetAddressCommand(serviceProvider.GetService<EmployeeService>());
                    result = setAddress.Execute(commandArg);
                    break;
                case "EmployeeInfo":
                    var employeeInfo = new EmployeeInfoCommand(serviceProvider.GetService<EmployeeService>());
                    result = employeeInfo.Execute(commandArg);
                    break;
                case "EmployeePersonalInfo":
                    var employeePersonalInfo = new EmployeePersonalInfoCommand(serviceProvider.GetService<EmployeeService>());
                    result = employeePersonalInfo.Execute(commandArg);
                    break;
                case "SetManager":
                    var setManager = new SetManagerCommand(serviceProvider.GetService<EmployeeService>());
                    result = setManager.Execute(commandArg);
                    break;
                case "ManagerInfo":
                    var managerInfo = new ManagerInfoCommand(serviceProvider.GetService<EmployeeService>());
                    result = managerInfo.Execute(commandArg);
                    break;
                case "ListEmployeesOlderThan":
                    var olderThan = new ListEmployeesOlderThanCommand(serviceProvider.GetService<EmployeeService>());
                    result = olderThan.Execute(commandArg);
                    break;
                default: throw new InvalidOperationException("Invalid command!");
            }
            return result;
        }
    }
}
