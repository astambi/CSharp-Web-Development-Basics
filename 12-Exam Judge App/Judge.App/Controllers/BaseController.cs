namespace Judge.App.Controllers
{
    using Data;
    using Data.Models;
    using SimpleMvc.Framework.Contracts;
    using SimpleMvc.Framework.Controllers;
    using System.Linq;

    public abstract class BaseController : Controller
    {
        protected const string HomePage = "/";
        protected const string LoginPage = "/users/login";
        protected const string ContestsPage = "/contests/all";
        protected const string SubmissionsPage = "/submissions/all";

        public BaseController()
        {
            this.ViewModel["anonymousDisplay"] = "flex";
            this.ViewModel["userDisplay"] = "none";
            this.ViewModel["adminDisplay"] = "none";
            this.ViewModel["show-error"] = "none";
        }

        protected User Profile { get; set; }

        protected bool IsAdmin => this.User.IsAuthenticated && this.Profile.IsAdmin;

        protected void ShowError(string error)
        {
            this.ViewModel["show-error"] = "block";
            this.ViewModel["error"] = error;
        }

        protected IActionResult RedirectToHome() => this.Redirect(HomePage);

        protected IActionResult RedirectToLogin() => this.Redirect(LoginPage);

        protected IActionResult RedirectToContests() => this.Redirect(ContestsPage);

        protected IActionResult RedirectToSubmissions() => this.Redirect(SubmissionsPage);

        protected override void InitializeController()
        {
            base.InitializeController();

            if (this.User.IsAuthenticated)
            {
                this.ViewModel["anonymousDisplay"] = "none";
                this.ViewModel["userDisplay"] = "flex";

                using (var context = new JudgeDbContext())
                {
                    this.Profile = context
                        .Users
                        .First(u => u.Email == this.User.Name);

                    if (this.Profile.IsAdmin)
                    {
                        this.ViewModel["adminDisplay"] = "flex";
                    }
                }
            }
        }
    }
}
