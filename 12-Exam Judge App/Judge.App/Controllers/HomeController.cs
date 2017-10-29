namespace Judge.App.Controllers
{
    using SimpleMvc.Framework.Contracts;

    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            // Guest Users
            this.ViewModel["guestDisplay"] = "block";
            this.ViewModel["authenticated"] = "none";

            // Authenticated Users
            if (this.User.IsAuthenticated)
            {
                this.ViewModel["guestDisplay"] = "none";
                this.ViewModel["authenticated"] = "block";

                this.ViewModel["user"] = string.IsNullOrWhiteSpace(this.Profile.FullName)
                                        ? this.Profile.Email
                                        : this.Profile.FullName;
            }

            return this.View();
        }
    }
}
