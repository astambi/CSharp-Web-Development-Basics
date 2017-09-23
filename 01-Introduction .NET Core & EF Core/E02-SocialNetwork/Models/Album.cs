namespace SocialNetwork.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Album
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string BackgroundColor { get; set; }

        public bool IsPublic { get; set; }

        // Task 3
        public ICollection<PictureAlbum> Pictures { get; set; } = new List<PictureAlbum>();

        public int UserId { get; set; }

        public User User { get; set; }

        // Task 4
        public ICollection<AlbumTag> Tags { get; set; } = new List<AlbumTag>();
    }
}
