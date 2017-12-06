using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Instagraph.Data;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Instagraph.DataProcessor
{
    public class Serializer
    {
        public static string ExportUncommentedPosts(InstagraphContext context)
        {
            var posts = context.Posts
                .Where(p => p.Comments.Count == 0)
                .OrderBy(p => p.Id)
                .Select(p => new
                {
                    Id = p.Id,
                    Picture = p.Picture.Path,
                    User = p.User.Username
                })
                .ToList();

            var json = JsonConvert.SerializeObject(posts, Formatting.Indented);
            return json;
        }

        public static string ExportPopularUsers(InstagraphContext context)
        {
            var users = context.Users
                .Where(u => u.Posts
                    .Any(p => p.Comments
                        .Any(c => u.Followers
                            .Any(f => f.FollowerId == c.UserId))))
                .OrderBy(u => u.Id)
                .Select(u => new
                {
                    Username = u.Username,
                    Followers = u.Followers.Count
                })
                .ToList();

            var json = JsonConvert.SerializeObject(users, Formatting.Indented);
            return json;
        }

        public static string ExportCommentsOnPosts(InstagraphContext context)
        {
            var users = context.Users
                .Select(u => new
                {
                    u.Username,
                    MostComments = u.Posts.Max(p => (int?)p.Comments.Count) ?? 0
                })
                .OrderByDescending(u => u.MostComments)
                .ThenBy(u => u.Username)
                .ToList();

            var xDoc = new XDocument();
            var xElements = new List<XElement>();

            foreach (var user in users)
            {
                xElements.Add(new XElement("user",
                                new XElement("Username", user.Username),
                                new XElement("MostComments", user.MostComments))
                    );
            }

            xDoc.Add(new XElement("users", xElements));

            var xml = xDoc.ToString();

            return xml;
        }
    }
}
