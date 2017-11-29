using System;
using Forum.App.Commands.Contracts;

namespace Forum.App.Commands
{
    public class WhoAmICommand : ICommand
    {
        public string Execute(params string[] arguments)
        {
            if (Session.User == null)
            {
                return "Yoy are not logged in";
                
            }

            var currentUser = $"{Session.User.Username}";

            return $"{currentUser}";
        }
    }
}
