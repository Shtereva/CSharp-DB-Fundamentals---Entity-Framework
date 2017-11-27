using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PhotoShare.Client.Utilities;
using PhotoShare.Data;
using PhotoShare.Models;

namespace PhotoShare.Client.Core.Commands
{
    using System;

    public class CreateAlbumCommand
    {
        // CreateAlbum <username> <albumTitle> <BgColor> <tag1> <tag2>...<tagN>
        public string Execute(string[] data)
        {
            if (data.Length < 3)
            {
                throw new InvalidOperationException("Command CreateAlbum not valid!");
            }

            if (Session.LoggedUser == null || data[0] != Session.LoggedUser.Username)
            {
                throw new InvalidOperationException("Invalid credentials!");
            }

            string username = data[0];
            string albumTitle = data[1];
            string color = data[2];

            var tags = data.Skip(3).ToList();

            for (int i = 0; i < tags.Count; i++)
            {
                tags[i] = tags[i].ValidateOrTransform();
            }

            using (var context = new PhotoShareContext())
            {
                var user = context.Users
                    .Include(u => u.AlbumRoles)
                    .ThenInclude(ar => ar.Album)
                    .ThenInclude(a => a.AlbumTags)
                    .SingleOrDefault(u => u.Username == username);


                if (user == null)
                {
                    throw new ArgumentException($"User {username} not found!");
                }

                if (user.AlbumRoles.Select(a => a.Album).SingleOrDefault(a => a.Name == albumTitle) != null)
                {
                    throw new ArgumentException($"Album {albumTitle} exists!");
                }

                var colors = Enum.GetNames(typeof(Color)).ToList();
                var index = colors.IndexOf(color);

                if (colors.All(c => c != color))
                {
                    throw new ArgumentException($"Color {color} not found!");
                }

                if (!context.Tags.Any())
                {
                    throw new ArgumentException("Invalid tags!");
                }


                foreach (var tag in tags)
                {
                    if (!context.Tags.Any(t => t.Name == tag))
                    {
                        throw new ArgumentException("Invalid tags!");
                    }
                }

                var album = new Album()
                {
                    Name = albumTitle,
                    BackgroundColor = (Color)index
                    
                };

                context.Albums.Add(album);
                context.SaveChanges();

                var list = new List<AlbumTag>();

                foreach (var tag in tags)
                {
                    var userTag = new AlbumTag()
                    {
                        Album = album,
                        Tag = context.Tags.SingleOrDefault(t => t.Name == tag)
                    };

                    list.Add(userTag);
                }

                var newAlbum = context.Albums
                    .Include(a => a.AlbumTags)
                    .SingleOrDefault(a => a.Name == albumTitle);

                newAlbum.AlbumTags.ToList().AddRange(list);

                var role = new AlbumRole()
                {
                    Album = newAlbum,
                    User = user,
                    Role = (Role)0
                };

                user.AlbumRoles.Add(role);

                context.SaveChanges();
            }

            return $"Album {albumTitle} successfully created!";
        }
    }
}
