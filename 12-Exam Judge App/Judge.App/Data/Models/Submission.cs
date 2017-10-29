namespace Judge.App.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Submission
    {
        public int Id { get; set; }

        [Required]
        public string Code { get; set; }

        public int ContestId { get; set; }

        public Contest Contest { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public BuildType Build { get; set; } = BuildType.Failed;
    }
}
