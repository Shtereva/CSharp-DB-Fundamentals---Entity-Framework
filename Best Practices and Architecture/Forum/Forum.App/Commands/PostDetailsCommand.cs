using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Forum.App.Commands.Contracts;
using Forum.App.Models;
using Forum.Models;
using Forum.Services.Contracts;

namespace Forum.App.Commands
{
    class PostDetailsCommand : ICommand
    {
        private readonly IPostService postService;

        public PostDetailsCommand(IPostService postService)
        {
            this.postService = postService;
        }

        public string Execute(params string[] arguments)
        {
            var postId = int.Parse(arguments[0]);

            if (Session.User == null)
            {
                return "You are not logged in";
            }

            var post = postService.ById<PostDetailsDto>(postId);

            var sb = new StringBuilder();
            sb.AppendLine($"{post.Title} by {post.AuthorUsername}");

            foreach (var reply in post.Replies)
            {
                sb.AppendLine($"--Reply from {reply.AuthorUsername} - {reply.Content}");
            }

            return sb.ToString();
        }
    }
}
