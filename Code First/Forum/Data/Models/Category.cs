using System.Collections;
using System.Collections.Generic;

namespace Forum.Data.Models
{
    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<Post> Posts { get; set; }
    }
}
