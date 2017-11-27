using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PhotoShare.Data;
using PhotoShare.Models;

namespace PhotoShare.Client.Core.Commands
{
    using System;

    public class PrintFriendsListCommand 
    {
        // PrintFriendsList <username>
        public string Execute(string[] data)
        {
            if (data.Length != 1)
            {
                throw new InvalidOperationException("Command ListFriends not valid!");
            }

            string username = data[0];
            string result = string.Empty;
            var friends = new List<User>();

            using (var context = new PhotoShareContext())
            {
                var user = context.Users
                    .Include(u => u.FriendsAdded)
                    .SingleOrDefault(u => u.Username == username);

                if (user == null)
                {
                    throw new ArgumentException($"User {username} not found!");
                }

                foreach (var friend in user.FriendsAdded)
                {
                    var userFriend = context.Users.Find(friend.FriendId);
                    friends.Add(userFriend);
                }


                if (friends.Count == 0)
                {
                    return "No friends for this user. :(";
                }

                result = string.Join(Environment.NewLine, friends.Select(f => "-" + f.Username).OrderBy(f => f).ToList());
            }

            return $"Friends:{Environment.NewLine}{result}";
        }
    }
}
