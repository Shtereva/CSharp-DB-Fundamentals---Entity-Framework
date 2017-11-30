using System;
using System.Linq;
using Employees.App.Commands;

namespace Employees.App
{
    public class CommandDispatcher
    {
        public string DispatchCommand(string[] commandParams)
        {
            string commandName = commandParams[0];

            var commandArg = commandParams.Skip(1).ToArray();

            string result = string.Empty;

            switch (commandName)
            {
                case "AddEmployee":
                    var addEmployee = new AddEmployeeCommand();
                    result = addEmployee.Execute(commandArg);
                    break;
                case "SetBirthday":
                    var setBirthday = new SetBirthdayCommand();
                    result = setBirthday.Execute(commandArg);
                    break;
                case "SetAddress":
                    var setAddress = new SetAddressCommand();
                    result = setAddress.Execute(commandArg);
                    break;
                case "EmployeeInfo":
                    var employeeInfo = new EmployeeInfoCommand();
                    result = employeeInfo.Execute(commandArg);
                    break;
                case "EmployeePersonalInfo":
                    var employeePersonalInfo = new EmployeePersonalInfoCommand();
                    result = employeePersonalInfo.Execute(commandArg);
                    break;
                case "SetManager":
                    var setManager = new SetManagerCommand();
                    result = setManager.Execute(commandArg);
                    break;
                case "ManagerInfo":
                    var managerInfo = new ManagerInfoCommand();
                    result = managerInfo.Execute(commandArg);
                    break;
                case "ListEmployeesOlderThan":
                    var olderThan = new ListEmployeesOlderThanCommand();
                    result = olderThan.Execute(commandArg);
                    break;
                default: throw new InvalidOperationException("Invalid command!");
            }
            return result;
        }
    }
}
