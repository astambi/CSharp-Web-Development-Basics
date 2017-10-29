namespace Judge.App.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Contest
    {
        public int Id { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(100)]
        public string Name { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public ICollection<Submission> Submissions { get; set; } = new List<Submission>();
    }
}
