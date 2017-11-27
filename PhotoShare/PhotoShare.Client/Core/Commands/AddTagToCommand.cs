using System.Linq;
using Microsoft.EntityFrameworkCore;
using PhotoShare.Client.Utilities;
using PhotoShare.Data;
using PhotoShare.Models;

namespace PhotoShare.Client.Core.Commands
{
    using System;

    public class AddTagToCommand 
    {
        // AddTagTo <albumName> <tag>
        public string Execute(string[] data)
        {
            if (data.Length != 2)
            {
                throw new InvalidOperationException("Command AddTagTo not valid!");
            }

            if (Session.LoggedUser == null)
            {
                throw new InvalidOperationException("Invalid credentials!");
            }

            string albumName = data[0];
            string tagName = data[1].ValidateOrTransform();

            using (var context = new PhotoShareContext())
            {
                if (!context.Tags.Any(t => t.Name == tagName) || !context.Albums.Any(a => a.Name == albumName))
                {
                    throw new ArgumentException("Either tag or album do not exist!");
                }

                var album = context.Users
                    .Include(u => u.AlbumRoles)
                    .ThenInclude(ar => ar.Album)
                    .ThenInclude(a => a.AlbumTags)
                    .SingleOrDefault(u => u.Id == Session.LoggedUser.Id)
                    .AlbumRoles
                    .SingleOrDefault(a => a.Album.Name == albumName);

                if (album == null || album.Role != Role.Owner)
                {
                    throw new InvalidOperationException("Invalid credentials!");
                }
                var albumTag = new AlbumTag()
                {
                    Album = album.Album,
                    Tag = new Tag() { Name = tagName}
                };

                album.Album.AlbumTags.Add(albumTag);
            }

            return $"Tag {tagName} added to {albumName}!";
        }
    }
}
