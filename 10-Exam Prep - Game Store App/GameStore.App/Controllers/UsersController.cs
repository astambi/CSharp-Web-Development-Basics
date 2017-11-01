namespace GameStore.App.Controllers
{
    using Models.Users;
    using Services.Contracts;
    using SimpleMvc.Framework.Attributes.Methods;
    using SimpleMvc.Framework.Contracts;

    public class UsersController : BaseController
    {
        private const string RegisterError = @"<p>Check your form for errors!</p><p>E-mail must contain @ sign and a period.</p><p>Password must be at least 6 symbols and must contain at least 1 uppercase, 1 lowercase letter and 1 digit.</p><p>Confirm Password must match the provided password.</p>";
        private const string EmailExistsError = "E-mail is already taken!";
        private const string RequiredEmailAndPasswordError = "E-mail and password are required!";
        private const string InvalidUserCredentials = "Invalid e-mail or password!";

        private readonly IUserService userService;

        public UsersController(IUserService userService)
        {
            this.userService = userService;
        }

        public IActionResult Register()
        {
            // Authenticate user
            if (this.User.IsAuthenticated)
            {
                return this.Redirect(HomePage);
            }

            return this.View();
        }

        [HttpPost]
        public IActionResult Register(RegisterModel model)
        {
            // Validate model
            if (!this.IsValidModel(model) ||
                model.Password != model.ConfirmPassword)
            {
                this.ShowError(RegisterError);
                return this.View();
            }

            // Save user to db
            var result = this.userService.Create(model.Email, model.Password, model.FullName);

            // Unique email error
            if (!result)
            {
                this.ShowError(EmailExistsError);
                return this.View();
            }

            // Redirect to Login 
            return this.Redirect(LoginPage);
        }

        public IActionResult Login()
        {
            // Authenticate user
            if (this.User.IsAuthenticated)
            {
                return this.Redirect(HomePage);
            }

            return this.View();
        }

        [HttpPost]
        public IActionResult Login(LoginModel model)
        {
            // Validate model
            if (!this.IsValidModel(model))
            {
                this.ShowError(RequiredEmailAndPasswordError);
                return this.View();
            }

            // Validate user in db
            var usersExists = this.userService
                            .UserExists(model.Email, model.Password);

            if (!usersExists)
            {
                this.ShowError(InvalidUserCredentials);
                return this.View();
            }

            // SignIn
            this.SignIn(model.Email);

            // Redirect to Home
            return this.Redirect(HomePage);
        }

        public IActionResult Logout()
        {
            this.SignOut();

            return this.Redirect(HomePage);
        }
    }
}
