using System.Collections;
using System.Collections.Generic;

namespace Forum.Data.Models
{
    public class Post
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public int AuthorId { get; set; }
        public User Author { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public ICollection<Reply> Replies { get; set; }

        public ICollection<PostTag> PostTags { get; set; } = new List<PostTag>();
    }
}
