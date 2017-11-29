using System;
using System.Linq;
using System.Text;
using Forum.App.Commands.Contracts;
using Forum.Services.Contracts;

namespace Forum.App.Commands
{
    public class ListPostsCommand : ICommand
    {
        private readonly IPostService postService;

        public ListPostsCommand(IPostService postService)
        {
            this.postService = postService;
        }

        public string Execute(params string[] arguments)
        {
            var posts = postService.All()
                .GroupBy(p => p.Category.Name)
                //.Select(p => $"{p.Id} {p.Title} - {p.Content} by {p.Author.Username} in category {p.Category.Name}")
                .ToList();

            var sb = new StringBuilder();

            foreach (var group in posts)
            {
                var categoryName = group.Key;

                sb.AppendLine(categoryName + ": ");

                foreach (var post in group)
                {
                    sb.AppendLine(
                        $"{post.Id} {post.Title} - {post.Content} by {post.Author.Username} in category {post.Category.Name}");
                }
            }

            return sb.ToString();
        }
    }
}
