namespace WebServer.GameStoreApplication.Controllers
{
    using Common;
    using Infrastructure;
    using Server.Http;
    using Server.Http.Contracts;
    using Services;
    using Services.Contracts;

    public abstract class BaseController : Controller
    {
        protected const string HomePath = "/";

        private readonly IUserService users;

        protected BaseController(IHttpRequest request)
        {
            this.Request = request;
            this.Authentication = new Authentication(false, false);
            this.users = new UserService();

            this.ApplyAuthentication();
        }

        protected override string ApplicationDirectory => "GameStoreApplication";

        protected Authentication Authentication { get; private set; }

        protected IHttpRequest Request { get; private set; }

        private void ApplyAuthentication()
        {
            // Anonymous Users
            var anonymousDisplay = "flex";
            var authDisplay = "none";
            var adminDisplay = "none";

            var authenticatedUserEmail = this.Request
                .Session
                .Get<string>(SessionStore.CurrentUserKey);

            // Logged in user
            if (authenticatedUserEmail != null)
            {
                anonymousDisplay = "none";
                authDisplay = "flex";

                // Admin user
                var isAdmin = this.users.IsAdmin(authenticatedUserEmail);
                if (isAdmin)
                {
                    adminDisplay = "flex";
                }

                this.Authentication = new Authentication(true, isAdmin);
            }

            this.ViewData["anonymousDisplay"] = anonymousDisplay;
            this.ViewData["authDisplay"] = authDisplay;
            this.ViewData["adminDisplay"] = adminDisplay;
        }
    }
}
