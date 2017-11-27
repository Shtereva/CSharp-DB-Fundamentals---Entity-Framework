using System.Linq;
using Microsoft.EntityFrameworkCore;
using PhotoShare.Data;
using PhotoShare.Models;

namespace PhotoShare.Client.Core.Commands
{
    using System;

    public class UploadPictureCommand
    {
        // UploadPicture <albumName> <pictureTitle> <pictureFilePath>
        public string Execute(string[] data)
        {
            if (data.Length != 3)
            {
                throw new InvalidOperationException("Command UploadPicture not valid!");
            }

            if (Session.LoggedUser == null)
            {
                throw new InvalidOperationException("Invalid credentials!");
            }

            string albumName = data[0];
            string pictureTitle = data[1];
            string path = data[2];

            using (var context = new PhotoShareContext())
            {
                var album = context.Users
                    .Include(u => u.AlbumRoles)
                    .ThenInclude(a => a.Album)
                    .SingleOrDefault(u => u.Id == Session.LoggedUser.Id)
                    .AlbumRoles
                    .SingleOrDefault(a => a.Album.Name == albumName);

                if (album == null)
                {
                    throw new ArgumentException($"Album {albumName} not found!");
                }

                var picture = new Picture()
                {
                    Album = album.Album,
                    Title = pictureTitle,
                    Path = path
                };

                if (album.Album.Pictures.Any(a => a.Title == pictureTitle))
                {
                    throw new InvalidOperationException($"Picture {pictureTitle} is already added to album {albumName}");
                }

                if (album.Role != Role.Owner)
                {
                    throw new InvalidOperationException("Invalid credentials!");
                }
                album.Album.Pictures.Add(picture);

                context.SaveChanges();
            }

            return $"Picture {pictureTitle} added to {albumName}!";
        }
    }
}
