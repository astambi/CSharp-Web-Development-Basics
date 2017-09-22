using System;
using System.Collections.Generic;

namespace DemoDatabaseFirst.Models
{
    public partial class Comment
    {
        public int CommentId { get; set; }
        public int UserId { get; set; }
        public int PostId { get; set; }
        public string Content { get; set; }

        public Post Post { get; set; }
        public User User { get; set; }
    }
}
