namespace Judge.App.Models.Submissions
{
    using Infrastructure.Validation;
    using Infrastructure.Validation.Submissions;

    public class SubmissionModel
    {
        [Required]
        [Code]
        public string Code { get; set; }

        [Required]
        public int ContestId { get; set; }
    }
}
