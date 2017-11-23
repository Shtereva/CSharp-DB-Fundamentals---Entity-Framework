using System.Reflection;
using Forum.App.Commands.Contracts;
using Forum.Services.Contracts;

namespace Forum.App.Commands
{
    public class LoginCommand : ICommand
    {
        private readonly IUserService userService;

        public LoginCommand(IUserService userService)
        {
            this.userService = userService;
        }

        public string Execute(params string[] arguments)
        {
            string username = arguments[0];
            string password = arguments[1];

            var user = userService.ByUsernameAndPassword(username, password);

            Session.User = user;

            return "logged in succsessfully";
        }
    }
}
