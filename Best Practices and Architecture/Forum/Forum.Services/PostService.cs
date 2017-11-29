using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Forum.Data;
using Forum.Models;
using Forum.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;

namespace Forum.Services
{
    public class PostService : IPostService
    {
        private readonly ForumDbContext context;

        public PostService(ForumDbContext context)
        {
            this.context = context;
        }


        public TModel Create<TModel>(string title, string content, int categoryId, int authorId)
        {
            var post = new Post()
            {
                Title = title,
                Content = content,
                CategoryId = categoryId,
                AuthorId = authorId
            };

            context.Posts.Add(post);
            context.SaveChanges();

            var dto = Mapper.Map<TModel>(post);

            return dto;
        }

        public IQueryable<TModel> All<TModel>()
        {
            var posts = context.Posts
                .ProjectTo<TModel>();

            return posts;
        }

        public TModel ById<TModel>(int postId)
        {
            var post = context.Posts
                .Where(p => p.Id == postId)
                .ProjectTo<TModel>()
                .SingleOrDefault();

            return post;
        }
    }
}
 