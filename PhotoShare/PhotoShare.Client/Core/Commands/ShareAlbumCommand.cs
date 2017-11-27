using System.Linq;
using Microsoft.EntityFrameworkCore;
using PhotoShare.Data;
using PhotoShare.Models;

namespace PhotoShare.Client.Core.Commands
{
    using System;

    public class ShareAlbumCommand
    {
        // ShareAlbum <albumId> <username> <permission>
        // For example:
        // ShareAlbum 4 dragon321 Owner
        // ShareAlbum 4 dragon11 Viewer
        public string Execute(string[] data)
        {
            if (data.Length != 3)
            {
                throw new InvalidOperationException("Command ShareAlbum not valid!");
            }

            int albumId = 0;
            bool isValid = int.TryParse(data[0], out albumId);
            string username = data[1];
            string permission = data[2];
            string albumName = string.Empty;

            if (Session.LoggedUser == null)
            {
                throw new InvalidOperationException("Invalid credentials!");
            }

            var permissions = Enum.GetNames(typeof(Role)).ToList();
            int index = permissions.IndexOf(permission);

            using (var context = new PhotoShareContext())
            {
                var user = context.Users.SingleOrDefault(u => u.Username == username);

                if (user == null)
                {
                    throw new ArgumentException($"User {username} not found!");
                }

                if (!isValid || context.Albums.All(a => a.Id != albumId) )
                {
                    throw new ArgumentException($"Album {data[0]} not found!");
                }

                if (!permissions.Contains(permission))
                {
                    throw new ArgumentException("Permission must be either “Owner” or “Viewer”!");
                }

                var album = context.Albums.Include(a => a.AlbumRoles).SingleOrDefault(a => a.Id == albumId);

                var albumRole = new AlbumRole()
                {
                    Album = album,
                    User = user,
                    Role = (Role)index
                };

                if (context.AlbumRoles.Any(r => r.Album == album && r.User == user))
                {
                    throw new InvalidOperationException($"User {username} is already part of Album {albumName}");
                }

                album.AlbumRoles.Add(albumRole);

                context.SaveChanges();
                albumName = album.Name;
            }

            return $"Username {username} added to album {albumName} ({permission})";
        }
    }
}
