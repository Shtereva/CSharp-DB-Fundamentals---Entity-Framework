using System.Linq;
using Microsoft.EntityFrameworkCore;
using PhotoShare.Data;
using PhotoShare.Models;

namespace PhotoShare.Client.Core.Commands
{
    using System;

    public class AddFriendCommand
    {
        // AddFriend <username1> <username2>
        public string Execute(string[] data)
        {
            if (data.Length != 2)
            {
                throw new InvalidOperationException("Command AddFriend not valid!");
            }

            if (Session.LoggedUser == null || data[0] != Session.LoggedUser.Username)
            {
                throw new InvalidOperationException("Invalid credentials!");
            }

            using (var context = new PhotoShareContext())
            {
                var user1 = context.Users
                    .Include(u => u.FriendsAdded)
                    .Include(u => u.AddedAsFriendBy)
                    .SingleOrDefault(u => u.Username == data[0]);

                var user2 = context.Users
                    .Include(u => u.FriendsAdded)
                    .Include(u => u.AddedAsFriendBy)
                    .SingleOrDefault(u => u.Username == data[1]);

                if (user1 == null || user2 == null || user1.IsDeleted == true || user2.IsDeleted == true)
                {
                    throw new ArgumentException(user1 == null || user1.IsDeleted == true
                                                    ? $"{data[0]} not found!" : $"{data[1]} not found!");
                }

                if (user1.Username == user2.Username)
                {
                    throw new InvalidOperationException("Invalid friend request!");
                }

                if (user1.FriendsAdded.Any(u => u.FriendId == user2.Id) && user2.FriendsAdded.All(u => u.FriendId != user1.Id))
                {
                    throw new InvalidOperationException($"You have already sent a request to {user2.Username}");
                }

                if (user1.FriendsAdded.Any(u => u.FriendId == user2.Id) && user2.FriendsAdded.Any(u => u.FriendId == user1.Id))
                {
                    throw new InvalidOperationException($"{user2.Username} is already a friend to {user1.Username}");
                }

                user1.FriendsAdded.Add(new Friendship() { User = user1, Friend = user2 });

                context.SaveChanges();
            }

            return $"Friend {data[1]} added to {data[0]}";
        }
    }
}
