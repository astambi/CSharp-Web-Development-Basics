namespace Judge.App.Controllers
{
    using Infrastructure;
    using Models.Contests;
    using Services.Contracts;
    using SimpleMvc.Framework.Attributes.Methods;
    using SimpleMvc.Framework.Contracts;
    using System.Linq;

    public class ContestsController : BaseController
    {
        private const string ContestError = @"<p>Contest name must begin with uppercase letter and has length between 3 and 100 symbols.</p>";
        private const string ContestNotFoundError = "Contest # {0} does not exist in the dababase";

        private readonly IContestService contestService;

        public ContestsController(IContestService contestService)
        {
            this.contestService = contestService;
        }

        public IActionResult Create()
        {
            // Authenticate users
            if (!this.User.IsAuthenticated)
            {
                return this.RedirectToLogin();
            }

            return this.View();
        }

        [HttpPost]
        public IActionResult Create(ContestModel model)
        {
            // Authenticate users
            if (!this.User.IsAuthenticated)
            {
                return this.RedirectToLogin();
            }

            // Validate model
            if (!this.IsValidModel(model))
            {
                this.ShowError(ContestError);
                return this.View();
            }

            // Create contest in db
            this.contestService.Create(model.Name, this.Profile.Id);

            return this.RedirectToContests();
        }

        public IActionResult All()
        {
            // Authenticate users
            if (!this.User.IsAuthenticated)
            {
                return this.RedirectToLogin();
            }

            // Get contests from db, prepare view
            var contests = this.contestService
                .All()
                .Select(c => c.ToHtml(
                    this.IsAdmin || 
                    this.IsOwner(c.Id)))
                .ToList();

            this.ViewModel["contests"] = string.Join(string.Empty, contests);

            return this.View();
        }

        public IActionResult Edit(int id)
        {
            // Accessible to Admins & Owners only
            if (!this.IsAdmin &&
                !this.IsOwner(id))
            {
                return this.RedirectToContests();
            }

            // Get contest from db
            var contest = this.contestService.GetById(id);

            // Not Found error
            if (contest == null)
            {
                this.ShowError(string.Format(ContestNotFoundError, id));
                return this.All();
            }

            // Prepare ViewModel
            this.GetContestViewData(contest);

            return this.View();
        }

        [HttpPost]
        public IActionResult Edit(int id, ContestModel model)
        {
            // Accessible to Admins & Owners only
            if (!this.IsAdmin &&
                !this.IsOwner(id))
            {
                return this.RedirectToContests();
            }

            // Validate model
            if (!this.IsValidModel(model))
            {
                this.ShowError(ContestError);
                this.GetContestViewData(model); // update post view data
                return this.View();
            }

            // Update contest in db
            var result = this.contestService.Update(
                id,
                model.Name);

            // Not Found error
            if (!result)
            {
                this.ShowError(string.Format(ContestNotFoundError, id));
                return this.All();
            }

            return this.RedirectToContests();
        }

        public IActionResult Delete(int id)
        {
            // Accessible to Admins & Owners only
            if (!this.IsAdmin &&
                !this.IsOwner(id))
            {
                return this.RedirectToLogin();
            }

            // Get contest from db
            var contest = this.contestService.GetById(id);

            // Not Found error
            if (contest == null)
            {
                this.ShowError(string.Format(ContestNotFoundError, id));
                return this.All();
            }

            // Prepare ViewModel
            this.GetContestViewData(contest);
            this.ViewModel["id"] = id.ToString();

            return this.View();
        }

        [HttpPost]
        public IActionResult ConfirmDelete(int id)
        {
            // Accessible to Admins & Owners only
            if (!this.IsAdmin &&
                !this.IsOwner(id))
            {
                return this.RedirectToContests();
            }

            // Get from db
            var contest = this.contestService.GetById(id);

            // Remove from db
            var result = this.contestService.Delete(id);

            // Not Found error
            if (!result)
            {
                this.ShowError(string.Format(ContestNotFoundError, id));
                return this.All();
            }

            return this.RedirectToContests();
        }

        private void GetContestViewData(ContestModel model)
        {
            this.ViewModel["name"] = model.Name;
        }

        private bool IsOwner(int contestId)
        {
            return this.contestService.IsOwner(contestId, this.Profile.Id);
        }
    }
}
