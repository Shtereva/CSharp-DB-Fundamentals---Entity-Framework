using System.Linq;
using Microsoft.EntityFrameworkCore;
using PhotoShare.Data;
using PhotoShare.Models;

namespace PhotoShare.Client.Core.Commands
{
    using System;

    public class AcceptFriendCommand
    {
        // AcceptFriend <username1> <username2>
        public string Execute(string[] data)
        {
            if (data.Length != 2)
            {
                throw new InvalidOperationException("Command AcceptFriend not valid!");
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
                        ? $"{data[0]} not found!"
                        : $"{data[1]} not found!");
                }

                if (user1.Username == user2.Username)
                {
                    throw new InvalidOperationException("You can't add yourself as a friend!");
                }

                if (user1.FriendsAdded.Any(u => u.FriendId == user2.Id) && user2.FriendsAdded.Any(u => u.FriendId == user1.Id))
                {
                    throw new InvalidOperationException($"{user2.Username} is already a friend to {user1.Username}");
                }

                if (user2.FriendsAdded.All(u => u.Friend.Username != user1.Username))
                {
                    throw new InvalidOperationException($"{user2.Username} has not added {user1.Username} as a friend");
                }

                user1.FriendsAdded.Add(new Friendship() { User = user1, Friend = user2 });

                context.SaveChanges();
            }

            return $"{data[0]} accepted {data[1]} as a friend";
        }
    }
}
