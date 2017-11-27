using System;
using System.Linq;
using PhotoShare.Data;

namespace PhotoShare.Client.Core.Commands
{
    public class LoginCommand
    {
        // Login <username> <password>

        public string Execute(string[] data)
        {
            if (data.Length != 2)
            {
                throw new InvalidOperationException("Command Login not valid!");
            }

            string username = data[0];
            string password = data[1];

            using (var context = new PhotoShareContext())
            {
                var user = context.Users.SingleOrDefault(u => u.Username == username);

                if (user == null || user.Password != password)
                {
                    throw new ArgumentException("Invalid username or password!");
                }

                if (Session.LoggedUser != null)
                {
                    throw new ArgumentException("Invalid Credentials!");
                }

                Session.LoggedUser = user;
            }

            return $"User {username} successfully logged in!";
        }
    }
}
