using Forum.App.Commands.Contracts;
using Forum.Models;
using Forum.Services.Contracts;

namespace Forum.App.Commands
{
    public class RegisterCommand : ICommand
    {
        private readonly IUserService userService;

        public RegisterCommand(IUserService userService)
        {
            this.userService = userService;
        }
        public string Execute(params string[] arguments)
        {
            string username = arguments[0];
            string password = arguments[1];

            var existingUser = userService.ByUsername<User>(username);
            if (existingUser != null)
            {
                return "There is already an exisitng user with that username.";
            }

            userService.Create<User>(username, password);

            return "Registration successfull";
        }
    }
}
