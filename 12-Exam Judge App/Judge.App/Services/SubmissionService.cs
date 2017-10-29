namespace Judge.App.Services
{
    using Contracts;
    using Data;
    using Data.Models;
    using Models.Submissions;
    using System.Collections.Generic;
    using System.Linq;

    public class SubmissionService : ISubmissionService
    {
        private readonly JudgeDbContext context;

        public SubmissionService(JudgeDbContext context)
        {
            this.context = context;
        }

        public void Create(string code, int contestId, int userId, BuildType build)
        {
            var submission = new Submission
            {
                Code = code,
                ContestId = contestId,
                UserId = userId,
                Build = build
            };

            this.context.Submissions.Add(submission);
            this.context.SaveChanges();
        }

        public IEnumerable<SubmissionListingModel> AllOwnedByUser(int userId)
        {
            return this.context
                .Contests
                .Where(c => c.Submissions.Any(s => s.UserId == userId))
                .Select(c => new SubmissionListingModel
                {
                    ContestId = c.Id,
                    Contest = c.Name,
                    SubmissionsBuild = c.Submissions
                                        .Where(s => s.UserId == userId)
                                        .OrderByDescending(s => s.Id)
                                        .Select(s => s.Build)
                                        .ToList()
                })
                .OrderByDescending(c => c.ContestId)
                .Distinct()
                .ToList();
        }
    }
}
