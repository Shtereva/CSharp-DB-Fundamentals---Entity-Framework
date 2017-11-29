using AutoMapper;
using Forum.Data;
using Forum.Models;
using Forum.Services.Contracts;

namespace Forum.Services
{
    public class ReplyService : IReplyService
    {
        private readonly ForumDbContext context;

        public ReplyService(ForumDbContext context)
        {
            this.context = context;
        }

        public TModel Create<TModel>(string replyText, int postId, int authorId)
        {
            var reply = new Reply()
            {
                AuthorId = authorId,
                Content = replyText,
                PostId = postId
            };

            context.Replies.Add(reply);
            context.SaveChanges();

            var dto = Mapper.Map<TModel>(reply);

            return dto;
        }

        public void Delete(int replyId)
        {
            var reply = context.Replies.Find(replyId);

            context.Replies.Remove(reply);
            context.SaveChanges();
        }
    }
}
