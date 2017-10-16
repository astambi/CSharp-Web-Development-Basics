namespace WebServer.GameStoreApplication.Controllers
{
    using Server.Http.Contracts;
    using Services;
    using Services.Contracts;
    using ViewModels.Account;
    using WebServer.Server.Http;

    public class AccountController : BaseController
    {
        private const string RegisterView = @"account\register";
        private const string LoginView = @"account\login";
        private const string ProfileView = @"account\profile";

        private readonly IUserService userService;

        public AccountController()
        {
            this.userService = new UserService();
        }

        public IHttpResponse Register()
        {
            this.SetDefaultView();

            return this.FileViewResponse(RegisterView);
        }

        public IHttpResponse Register(RegisterViewModel model)
        {
            // Validate model
            if (!this.ValidateModel(model))
            {
                return this.Register();
            }

            // Create User in db
            var success = this.userService.Create(model.Email, model.FullName, model.Password);
            if (!success)
            {
                this.AddError("E-mail is taken");

                return this.Register();
            }

            // Redirect to Login
            return this.RedirectResponse("/account/login");
        }

        public IHttpResponse Login()
        {
            this.SetDefaultView();

            return this.FileViewResponse(LoginView);
        }

        public IHttpResponse Login(IHttpRequest req, LoginViewModel model)
        {
            // Validate model
            if (!this.ValidateModel(model))
            {
                return this.Login();
            }

            // Find user in db
            var success = this.userService.Find(model.Email, model.Password);
            if (!success)
            {
                this.AddError("Invalid e-mail or password");

                return this.Login();
            }

            // Save sesssion
            req.Session.Add(SessionStore.CurrentUserKey, model.Email);

            // Redirect to Home
            return this.RedirectResponse("/");
        }

        public IHttpResponse Logout(IHttpRequest req)
        {
            req.Session.Clear();

            // Redirect to Home
            return this.RedirectResponse("/");
        }

        private void SetDefaultView()
        {
            this.ViewData["anonymousDisplay"] = "flex";
            this.ViewData["authDisplay"] = "none";

        }
    }
}
