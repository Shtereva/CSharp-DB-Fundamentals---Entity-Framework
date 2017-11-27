using System;

namespace PhotoShare.Client.Core.Commands
{
    public class LogoutCommand
    {
        // Logout

        public string Execute()
        {

            if (Session.LoggedUser == null)
            {
                return "You should log in first in order to logout.";
            }

            string username = Session.LoggedUser.Username;

            Session.LoggedUser = null;
            return $"User {username} successfully logged out!";
        }
    }
}
