using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Linq;

using Newtonsoft.Json;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

using Instagraph.Data;
using Instagraph.Models;
using Newtonsoft.Json.Linq;

namespace Instagraph.DataProcessor
{
    public class Deserializer
    {
        public static string ImportPictures(InstagraphContext context, string jsonString)
        {
            var pictures = JsonConvert.DeserializeObject<Picture[]>(jsonString);
            var sb = new StringBuilder();

            foreach (var picture in pictures)
            {
                if (!string.IsNullOrWhiteSpace(picture.Path) && picture.Size > 0 && !context.Pictures.Any(p => p.Path == picture.Path))
                {
                    context.Pictures.Add(picture);
                    context.SaveChanges();
                    sb.AppendLine($"Successfully imported Picture {picture.Path}.");
                    continue;
                }

                sb.AppendLine("Error: Invalid data.");
            }

            return sb.ToString();
        }

        public static string ImportUsers(InstagraphContext context, string jsonString)
        {
            var sb = new StringBuilder();

            var tokens = JToken.Parse(jsonString).Root;

            foreach (var token in tokens)
            {

                var username = token["Username"]?.Value<string>();
                var password = token["Password"]?.Value<string>();
                var profilePicture = token["ProfilePicture"]?.Value<string>();

                int pictureId = context.Pictures.SingleOrDefault(p => p.Path == profilePicture)?.Id ?? 0;

                if (context.Pictures.Any(p => p.Path == profilePicture) && !context.Users.Any(u => u.Username == username)
                    && username?.Length <= 30 && password?.Length <= 20)
                {
                    var user = new User()
                    {
                        Username = username,
                        Password = password,
                        ProfilePictureId = pictureId
                    };

                    context.Users.Add(user);
                    context.SaveChanges();
                    sb.AppendLine($"Successfully imported User {username}.");
                    continue;
                }

                sb.AppendLine("Error: Invalid data.");
            }

            return sb.ToString();
        }

        public static string ImportFollowers(InstagraphContext context, string jsonString)
        {
            var sb = new StringBuilder();

            var tokens = JToken.Parse(jsonString).Root;

            foreach (var token in tokens)
            {

                var user = token["User"]?.Value<string>();
                var follower = token["Follower"]?.Value<string>();

                int userId = context.Users.SingleOrDefault(u => u.Username == user)?.Id ?? 0;
                int followerId = context.Users.SingleOrDefault(u => u.Username == follower)?.Id ?? 0;


                if (context.Users.Any(u => u.Id == userId) && context.Users.Any(u => u.Id == followerId)
                    && !context.UsersFollowers.Any(u => u.UserId == userId && u.FollowerId == followerId))
                {
                    var userFolower = new UserFollower()
                    {
                        UserId = userId,
                        FollowerId = followerId
                    };

                    context.UsersFollowers.Add(userFolower);
                    sb.AppendLine($"Successfully imported Follower {follower} to User {user}.");
                    context.SaveChanges();
                    continue;
                }

                sb.AppendLine("Error: Invalid data.");
            }


            return sb.ToString();
        }

        public static string ImportPosts(InstagraphContext context, string xmlString)
        {
            var xml = XDocument.Parse(xmlString);
            var root = xml.Root.Elements();

            var sb = new StringBuilder();
            var posts = new List<Post>();

            foreach (var element in root)
            {
                string caption = element.Element("caption")?.Value;
                string user = element.Element("user")?.Value;
                string picture = element.Element("picture")?.Value;

                int userId = context.Users.SingleOrDefault(u => u.Username == user)?.Id ?? 0;
                int pictureId = context.Pictures.SingleOrDefault(p => p.Path == picture)?.Id ?? 0;

                if (context.Users.Any(u => u.Id == userId) && context.Pictures.Any(p => p.Id == pictureId) && caption != null)
                {
                    var post = new Post()
                    {
                        Caption = caption,
                        PictureId = pictureId,
                        UserId = userId
                    };

                    posts.Add(post);

                    sb.AppendLine($"Successfully imported Post {caption}.");
                    continue;
                }

                sb.AppendLine("Error: Invalid data.");
            }

            context.Posts.AddRange(posts);
            context.SaveChanges();

            return sb.ToString();
        }

        public static string ImportComments(InstagraphContext context, string xmlString)
        {
            var xml = XDocument.Parse(xmlString);
            var root = xml.Root.Elements();

            var sb = new StringBuilder();
            var comments = new List<Comment>();

            foreach (var element in root)
            {
                string content = element.Element("content")?.Value;
                string user = element.Element("user")?.Value;
                string post = element.Element("post")?.Attribute("id")?.Value ?? "0";

                int userId = context.Users.SingleOrDefault(u => u.Username == user)?.Id ?? 0;
                int postId = context.Posts.SingleOrDefault(p => p.Id == int.Parse(post))?.Id ?? 0;

                if (context.Users.Any(u => u.Id == userId) && context.Posts.Any(p => p.Id == postId) && content != null
                    && content.Length <= 250)
                {
                    var comment = new Comment()
                    {
                        Content = content,
                        PostId = postId,
                        UserId = userId
                    };

                    comments.Add(comment);

                    sb.AppendLine($"Successfully imported Comment {content}.");
                    continue;
                }

                sb.AppendLine("Error: Invalid data.");
            }

            context.Comments.AddRange(comments);
            context.SaveChanges();

            return sb.ToString();
        }
    }
}
