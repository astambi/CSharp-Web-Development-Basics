namespace Judge.App.Controllers
{
    using Data.Models;
    using Infrastructure;
    using Models.Submissions;
    using Services.Contracts;
    using SimpleMvc.Framework.Attributes.Methods;
    using SimpleMvc.Framework.Contracts;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class SubmissionsController : BaseController
    {
        private const string SubmissionError = "<p>Code length must be at least 3 symbols long </p>";

        private readonly ISubmissionService submissionService;
        private readonly IContestService contestService;
        private readonly Random random;

        public SubmissionsController(ISubmissionService submissionService,
                                     IContestService contestService)
        {
            this.submissionService = submissionService;
            this.contestService = contestService;
            this.random = new Random();
        }

        public IActionResult Create()
        {
            // Authenticate users
            if (!this.User.IsAuthenticated)
            {
                return this.RedirectToLogin();
            }

            // Get contests from db
            var contests = this.contestService
                .AllDropDown()
                .Select(c => c.ToHtml())
                .ToList();

            this.ViewModel["contests"] = string.Join(string.Empty, contests);

            return this.View();
        }

        [HttpPost]
        public IActionResult Create(SubmissionModel model)
        {
            // Authenticate users
            if (!this.User.IsAuthenticated)
            {
                return this.RedirectToLogin();
            }

            // Validate model
            if (!this.IsValidModel(model))
            {
                this.ShowError(SubmissionError);
                return this.View();
            }

            // Random processing with 70% chance for failure
            var randomValue = this.random.Next(0, 100);

            // Create in db
            this.submissionService.Create(
                model.Code,
                model.ContestId,
                this.Profile.Id,
                randomValue > 70 ? BuildType.Success : BuildType.Failed);

            return this.RedirectToSubmissions();
        }

        public IActionResult All()
        {
            // Authenticate users
            if (!this.User.IsAuthenticated)
            {
                return this.RedirectToLogin();
            }

            // Get contests from db
            var contestsData = this.submissionService.AllOwnedByUser(this.Profile.Id);

            this.PrepareContestsHtml(contestsData);
            this.PrepareSubmissionsHtml(contestsData);

            return this.View();
        }

        private void PrepareContestsHtml(IEnumerable<SubmissionListingModel> contestsData)
        {
            if (contestsData.Any())
            {
                var contestsHtml = contestsData
                    .Select(c => c.ToHtml())
                    .ToList();

                this.ViewModel["contests"] = string.Join(string.Empty, contestsHtml);
            }
            else
            {
                this.ViewModel["contests"] = "<p>You don't have any submissions!</p>";
            }
        }

        private void PrepareSubmissionsHtml(IEnumerable<SubmissionListingModel> contestsData)
        {
            if (contestsData.Any())
            {
                var submissionsHtml = new StringBuilder();
                var random = new Random();

                foreach (var contest in contestsData)
                {
                    submissionsHtml.Append($@"
                    <div class=""tab-pane fade"" id=""{contest.ContestId}"" role=""tabpanel"">
                        <ul class=""list-group"">");

                    foreach (var submission in contest.SubmissionsBuild)
                    {
                        var build = submission.ToString();
                        var style = submission == BuildType.Success 
                                    ? "success" 
                                    : "danger";

                        submissionsHtml
                            .Append($@"<li class=""list-group-item list-group-item-{style}"">Build {build}</li>");
                    }

                    submissionsHtml.Append($@"</ul></div>");
                }

                this.ViewModel["submissions"] = submissionsHtml.ToString();
            }
            else
            {
                this.ViewModel["submissions"] = string.Empty;
            }
        }
    }
}
