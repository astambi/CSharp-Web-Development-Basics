namespace SocialNetwork.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Attributes;

    public class User
    {
        public int Id { get; set; }

        [Required]
        [MinLength(4), MaxLength(30)]
        public string Username { get; set; }

        [Required]
        [MinLength(6), MaxLength(50)]
        [ValidPassword]
        public string Password { get; set; }

        [Required]
        [ValidEmail]
        public string Email { get; set; }

        // Replaced by PictureAlbums (Task 3)
        //[MaxLength(1024)]
        //public byte[] ProfilePicture { get; set; }

        public DateTime RegisteredOn { get; set; }

        public DateTime? LastTimeLoggedIn { get; set; }

        [Range(1, 120)]
        public int? Age { get; set; }

        public bool IsDeleted { get; set; }

        public ICollection<UserFriend> FriendshipsMade { get; set; } = new List<UserFriend>();

        public ICollection<UserFriend> FriendshipsAccepted { get; set; } = new List<UserFriend>();

        // Replaces ProfilePicture (Task 3)
        // Replaced by UserAlbum (Task 5)
        // Do not delete nav property! Used for data transfer from Albums to UserAlbums (Task 5)
        public ICollection<Album> Albums { get; set; } = new List<Album>();

        // Replaces Album (Task 5)
        public ICollection<UserAlbum> SharedAlbums { get; set; } = new List<UserAlbum>();
    }
}
