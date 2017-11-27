using System;
using System.Linq;

namespace PhotoShare.Client.Core.Commands
{
    using Models;
    using Data;
    using Utilities;

    public class AddTagCommand
    {
        // AddTag <tag>
        public string Execute(string[] data)
        {
            if (Session.LoggedUser == null)
            {
                throw new InvalidOperationException("Invalid credentials!");
            }

            string tagName = data.Length == 0 ? null : data[0];

            var tag = tagName.ValidateOrTransform();

            using (var context = new PhotoShareContext())
            {
                if (context.Tags.Any(t => t.Name == tag))
                {
                    throw new ArgumentException($"Tag {tag} exists!");
                }

                context.Tags.Add(new Tag
                {
                    Name = tag
                });

                context.SaveChanges();
            }

            return $"Tag {tag} was added successfully!";
        }
    }
}
