namespace PhotoShare.Client.Core.Commands
{
    using System;
    using System.Linq;

    using Data;

    public class DeleteUser
    {
        // DeleteUser <username>
        public string Execute(string[] data)
        {
            if (data.Length != 1)
            {
                throw new InvalidOperationException("Command DeleteUser not valid!");
            }

            string username = data[0];

            if (Session.LoggedUser == null || username != Session.LoggedUser.Username)
            {
                throw new InvalidOperationException("Invalid credentials!");
            }

            using (PhotoShareContext context = new PhotoShareContext())
            {
                var user = context.Users.SingleOrDefault(u => u.Username == username);

                if (user == null)
                {
                    throw new InvalidOperationException($"User {username} not found!");
                }

                if (user.IsDeleted == true)
                {throw new InvalidOperationException($"User {username} is already deleted!");
                }

                user.IsDeleted = true;

                context.SaveChanges();
            }

            return $"User {username} was deleted from the database!";
        }
    }
}
