namespace SimpleMvc.Domain
{
    using System.ComponentModel.DataAnnotations;

    public class Note
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        public int OwnerId { get; set; }

        public User Owner { get; set; }
    }
}
