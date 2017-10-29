namespace Judge.App.Models.Contests
{
    using Infrastructure.Validation;
    using Infrastructure.Validation.Contests;

    public class ContestModel
    {
        [Required]
        [Contest]
        public string Name { get; set; }
    }
}
