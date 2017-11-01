namespace ModPanel.App.Controllers
{
    using Data.Models;
    using Models.Users;
    using Services.Contracts;
    using SimpleMvc.Framework.Attributes.Methods;
    using SimpleMvc.Framework.Contracts;

    public class UsersController : BaseController
    {
        private const string RegisterError = @"<p>Check your form for errors!</p><p>E-mail must contain @ sign and a period.</p><p>Password must be at least 6 symbols and must contain at least 1 uppercase, 1 lowercase letter and 1 digit.</p><p>Confirm Password must match the provided password.</p>";
        private const string EmailIsTakenError = "E-mail is already taken!";
        private const string RequiredLoginError = "E-mail and password are required!";
        private const string UserIsNotApprovedError = "You must wait for your registration to be approved!";
        private const string InvalidUserCredentialsError = "Invalid e-mail or password!";

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
            if (!this.IsValidModel(model) ||
                model.Password != model.ConfirmPassword)
            {
                this.ShowError(RegisterError);
                return this.View();
            }

            // Save user in db
            var result = this.userService.Create(
                        model.Email,
                        model.Password,
                        (PositionType)model.Position);

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

            // Validate user in db & show errors
            // Validate user is approved 
            if (!this.userService.UserIsApproved(model.Email))
            {
                this.ShowError(UserIsNotApprovedError);
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
