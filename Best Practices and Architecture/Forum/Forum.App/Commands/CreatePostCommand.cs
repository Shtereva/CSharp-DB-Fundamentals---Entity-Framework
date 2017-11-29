using System;
using Forum.App.Commands.Contracts;
using Forum.App.Models;
using Forum.Models;
using Forum.Services.Contracts;

namespace Forum.App.Commands
{
    public class CreatePostCommand : ICommand
    {
        private readonly IPostService postService;
        private readonly ICategoryService categoryService;

        public CreatePostCommand(IPostService postService, ICategoryService categoryService)
        {
            this.postService = postService;
            this.categoryService = categoryService;
        }

        public string Execute(params string[] arguments)
        {
            var categoryName = arguments[0];
            var postTitle = arguments[1];
            var postContent = arguments[2];

            if (Session.User == null)
            {
                return "You are not logged in";
            }

            var category = categoryService.ByName<CategoryDto>(categoryName);

            if (category == null)
            {
               category = categoryService.Create<CategoryDto>(categoryName);
            }

            var post = postService.Create<PostDto>(postTitle, postContent, category.Id, Session.User.Id);

            return $"Post with id {post.Id} created successfully.";
        }
    }
}
