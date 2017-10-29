namespace Judge.App.Services.Contracts
{
    using Data.Models;
    using Models.Submissions;
    using System.Collections.Generic;

    public interface ISubmissionService
    {
        void Create(string code, int contestId, int userId, BuildType build);

        IEnumerable<SubmissionListingModel> AllOwnedByUser(int userId);
    }
}
