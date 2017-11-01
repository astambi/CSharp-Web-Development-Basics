namespace ModPanel.App.Controllers
{
    using Data;
    using Data.Models;
    using Services;
    using Services.Contracts;
    using SimpleMvc.Framework.Contracts;
    using SimpleMvc.Framework.Controllers;
    using System.Linq;

    public abstract class BaseController : Controller
    {
        protected const string HomePage = "/";
        protected const string LoginPage = "/users/login";

        protected const string PostError = @"<p>Check your form for errors!</p><p>Title must begin with uppercase letter and has length between 3 and 100 symbols.</p><p>Content should be at least 20 symbols long.</p>";

        private readonly ILogService logService;

        public BaseController()
        {
            this.logService = new LogService(new ModPanelDbContext());

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

        protected void Log(LogType type, string additionalInfo)
            => this.logService.Create(
                this.Profile.Email,
                type,
                additionalInfo);

        protected override void InitializeController()
        {
            base.InitializeController();

            if (this.User.IsAuthenticated)
            {
                this.ViewModel["anonymousDisplay"] = "none";
                this.ViewModel["userDisplay"] = "flex";

                using (var context = new ModPanelDbContext())
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
