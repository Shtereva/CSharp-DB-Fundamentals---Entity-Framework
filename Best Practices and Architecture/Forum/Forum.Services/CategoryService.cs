using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Forum.Data;
using Forum.Models;
using Forum.Services.Contracts;

namespace Forum.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ForumDbContext context;

        public CategoryService(ForumDbContext context)
        {
            this.context = context;
        }
        public TModel ByName<TModel>(string name)
        {
            var category = context.Categories
                .Where(c => c.Name == name)
                .ProjectTo<TModel>()
                .SingleOrDefault();

            return category;
        }

        public TModel Create<TModel>(string name)
        {
           var category = new Category()
           {
               Name = name
           };

            context.Categories.Add(category);
            context.SaveChanges();

            var dto = Mapper.Map<TModel>(category);

            return dto;
        }
    }
}
