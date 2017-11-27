using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;
using PhotoShare.Data;
using PhotoShare.Models;

namespace PhotoShare.Client.Core.Commands
{
    using System;

    public class ModifyUserCommand
    {
        // ModifyUser <username> <property> <new value>
        // For example:
        // ModifyUser <username> Password <NewPassword>
        // ModifyUser <username> BornTown <newBornTownName>
        // ModifyUser <username> CurrentTown <newCurrentTownName>
        // !!! Cannot change username
        public string Execute(string[] data)
        {
            if (data.Length != 3)
            {
                throw new InvalidOperationException("Command ModifyUser not valid!");
            }

            string username = data[0];
            string property = data[1];
            string value = data[2];

            if (Session.LoggedUser == null || username != Session.LoggedUser.Username)
            {
                throw new InvalidOperationException("Invalid credentials!");
            }

            using (var context = new PhotoShareContext())
            {
                var user = context.Users
                    .Include(u => u.BornTown)
                    .Include(u => u.CurrentTown)
                    .SingleOrDefault(u => u.Username == username);

                if (user == null)
                {
                    throw new ArgumentException($"User {username} not found!");
                }

                if (property.Contains("Town") && !context.Towns.Any(t => t.Name == value))
                {
                    throw new ArgumentException($"Value {value} not valid.{Environment.NewLine}Town {value} not found!");
                }
                switch (property)
                {
                    case "Password":
                        if (!value.Any(Char.IsDigit) || !value.Any(Char.IsLower) || value.Length < 6 || value.Length > 50)
                        {
                            throw new ArgumentException($"Value {value} not valid.{Environment.NewLine}Invalid Password");
                        }
                        user.Password = value;
                        break;
                    case "BornTown":
                        user.BornTownId = context.Towns.SingleOrDefault(t => t.Name == value).Id;
                        break;
                    case "CurrentTown":
                        user.CurrentTownId = context.Towns.SingleOrDefault(t => t.Name == value).Id;
                        break;
                        default: throw new ArgumentException($"Property {property} not supported!");

                }

                context.SaveChanges();
            }
            return $"User {username} {property} is {value}.";
        }
    }
}
