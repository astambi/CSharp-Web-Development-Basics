namespace Judge.App.Controllers
{
    using Models.Users;
    using Services.Contracts;
    using SimpleMvc.Framework.Attributes.Methods;
    using SimpleMvc.Framework.Contracts;
    using System.Linq;
    using System.Text;

    public class UsersController : BaseController
    {
        private const string EmailIsTakenError = "E-mail is already taken!";
        private const string RequiredLoginError = "E-mail and password are required!";
        private const string UserIsNotApprovedError = "You must wait for your registration to be approved!";
        private const string InvalidUserCredentialsError = "Invalid e-mail or password!";
        private const string EmailError = @"<p>E-mail must contain @ sign and a period.</p>";
        private const string PasswordError = @"<p>Password must be at least 6 symbols and must contain at least 1 uppercase, 1 lowercase letter and 1 digit.</p>";
        private const string ConfirmPasswordError = @"<p>Confirm Password must match the provided password.</p>";
        private const string AcceptanceError = @"<p>You have not accepted the Terms and Conditions.</p>";

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
                return this.RedirectToHome();
            }

            return this.View();
        }

        [HttpPost]
        public IActionResult Register(RegisterModel model)
        {
            // Authenticate user
            if (this.User.IsAuthenticated)
            {
                return this.RedirectToHome();
            }

            // Validate model
            var errors = new StringBuilder();
            if (!this.IsValidModel(model) ||
                model.Password != model.ConfirmPassword)
            {
                // Single error msg for every error
                if (string.IsNullOrEmpty(model.Email) ||
                    !model.Email.Contains("@") ||
                    !model.Email.Contains("."))
                {
                    errors.Append(EmailError);
                }

                if (string.IsNullOrEmpty(model.Password) ||
                   model.Password.Length < 6 ||
                   !model.Password.Any(ch => char.IsLower(ch)) ||
                   !model.Password.Any(ch => char.IsUpper(ch)) ||
                   !model.Password.Any(ch => char.IsDigit(ch)))
                {
                    errors.Append(PasswordError);
                }

                if (model.Password != model.ConfirmPassword)
                {
                    errors.Append(ConfirmPasswordError);
                }

                if (model.Accepted != "true")
                {
                    errors.Append(AcceptanceError);
                }

                this.ShowError(errors.ToString());
                return this.View();
            }

            // Save user in db
            var result = this.userService.Create(
                model.Email,
                model.Password,
                model.FullName);

            // Unique email error
            if (!result)
            {
                this.ShowError(EmailIsTakenError);
                return this.View();
            }

            // Redirect to Login 
            return this.RedirectToLogin();
        }

        public IActionResult Login()
        {
            // Authenticate user
            if (this.User.IsAuthenticated)
            {
                return this.RedirectToHome();
            }

            return this.View();
        }

        [HttpPost]
        public IActionResult Login(LoginModel model)
        {
            // Authenticate user
            if (this.User.IsAuthenticated)
            {
                return this.RedirectToHome();
            }

            // Validate model
            if (!this.IsValidModel(model))
            {
                this.ShowError(RequiredLoginError);
                return this.View();
            }

            // Validate user credentials
            if (!this.userService.UserExists(model.Email, model.Password))
            {
                this.ShowError(InvalidUserCredentialsError);
                return this.View();
            }

            // SignIn
            this.SignIn(model.Email);

            // Redirect to Home
            return this.RedirectToHome();
        }

        public IActionResult Logout()
        {
            this.SignOut();

            return this.RedirectToHome();
        }
    }
}
