namespace SocialNetwork.Models
{
    public class UserAlbum
    {
        public int UserId { get; set; }

        public User User { get; set; }

        public int AlbumId { get; set; }

        public Album Album { get; set; }

        // Task 6
        public UserRole UserRole { get; set; }
    }
}
