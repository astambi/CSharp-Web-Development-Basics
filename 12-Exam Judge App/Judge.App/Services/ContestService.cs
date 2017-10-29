namespace Judge.App.Services
{
    using Contracts;
    using Data;
    using Data.Models;
    using Models.Contests;
    using System.Collections.Generic;
    using System.Linq;

    public class ContestService : IContestService
    {
        private readonly JudgeDbContext context;

        public ContestService(JudgeDbContext context)
        {
            this.context = context;
        }

        public void Create(string name, int userId)
        {
            var contest = new Contest
            {
                Name = name,
                UserId = userId
            };

            this.context.Contests.Add(contest);
            this.context.SaveChanges();
        }

        public IEnumerable<ContestListingModel> All()
        {
            return this.context
                .Contests
                .Select(c => new ContestListingModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    SubmissionsCount = c.Submissions.Count
                })
                .ToList();
        }

        public IEnumerable<ContestDropDownModel> AllDropDown()
        {
            return this.context
                .Contests
                .Select(c => new ContestDropDownModel
                {
                    Id = c.Id,
                    Name = c.Name,
                })
                .ToList();
        }

        public ContestModel GetById(int id)
        {
            return this.context
                .Contests
                .Where(c => c.Id == id)
                .Select(c => new ContestModel
                {
                    Name = c.Name
                })
                .FirstOrDefault();
        }

        public bool Update(int id, string name)
        {
            // Find 
            var contest = this.context.Contests.Find(id);
            if (contest == null)
            {
                return false;
            }

            // Update 
            contest.Name = name;

            this.context.Update(contest);
            this.context.SaveChanges();

            return true;
        }

        public bool Delete(int id)
        {
            // Find 
            var contest = this.context.Contests.Find(id);
            if (contest == null)
            {
                return false;
            }

            // Remove all submissions belonging to the contest 
            var submissions = this.context
                .Submissions
                .Where(s => s.ContestId == contest.Id);

            if (submissions.Any())
            {
                this.context.RemoveRange(submissions);
            }

            // Remove contest from db
            this.context.Remove(contest);
            this.context.SaveChanges();

            return true;
        }

        public bool IsOwner(int id, int userId)
        {
            return this.context
               .Contests
               .Any(c => c.Id == id &&
                         c.UserId == userId);
        }
    }
}
