namespace Judge.App.Models.Submissions
{
    using Data.Models;
    using System.Collections.Generic;

    public class SubmissionListingModel
    {
        public int ContestId { get; set; }

        public string Contest { get; set; }

        public ICollection<BuildType> SubmissionsBuild { get; set; } = new List<BuildType>();
    }
}
