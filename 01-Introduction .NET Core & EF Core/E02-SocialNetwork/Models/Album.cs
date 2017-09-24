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

        // User navigation properties in Albums are not deleted on migration to SharesAlbums (UserAlbums)
        // Existing data in Albums is transfered to SharedAlbums

        // Do not delete nav property! Used for data transfer from Albums to UserAlbums (Task 5)
        public int UserId { get; set; }

        // Do not delete nav property! Used for data transfer from Albums to UserAlbums (Task 5)
        public User User { get; set; }

        // Task 4
        public ICollection<AlbumTag> Tags { get; set; } = new List<AlbumTag>();

        // Task 5
        public ICollection<UserAlbum> SharedAlbums { get; set; } = new List<UserAlbum>();
    }
}
