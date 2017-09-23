using System;
using System.Collections.Generic;

namespace DemoDatabaseFirst.Models
{
    public partial class Post
    {
        public Post()
        {
            Comments = new HashSet<Comment>();
        }

        public int PostId { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }

        public User User { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}
