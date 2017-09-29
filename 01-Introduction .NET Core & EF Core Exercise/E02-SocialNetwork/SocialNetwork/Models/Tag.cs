namespace SocialNetwork.Models
{
    using System.Collections.Generic;
    using Attributes;

    // Task 4
    public class Tag
    {
        public int Id { get; set; }

        [Tag]
        public string Name { get; set; }

        public ICollection<AlbumTag> Albums { get; set; } = new List<AlbumTag>();
    }
}
