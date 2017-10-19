namespace WebServer.GameStoreApplication.Controllers
{
    using Server.Http;
    using Server.Http.Contracts;
    using Services;
    using Services.Contracts;
    using ViewModels.Account;

    public class AccountController : BaseController
    {
        private const string RegisterView = @"account\register";
        private const string LoginView = @"account\login";
        private const string ProfileView = @"account\profile";

        private readonly IUserService userService;

        public AccountController(IHttpRequest request)
            : base(request)
        {
            this.userService = new UserService();
        }

        public IHttpResponse Register()
        {
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

            // Login User
            this.LoginUser(model.Email);

            // Redirect to Home
            return this.RedirectResponse(HomePath);
        }

        public IHttpResponse Login()
        {
            return this.FileViewResponse(LoginView);
        }

        public IHttpResponse Login(LoginViewModel model)
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
            LoginUser(model.Email);

            // Redirect to Home
            return this.RedirectResponse(HomePath);
        }

        public IHttpResponse Logout(IHttpRequest req)
        {
            req.Session.Clear();

            // Redirect to Home
            return this.RedirectResponse(HomePath);
        }

        private void LoginUser(string email)
        {
            this.Request.Session.Add(SessionStore.CurrentUserKey, email);
        }
    }
}
