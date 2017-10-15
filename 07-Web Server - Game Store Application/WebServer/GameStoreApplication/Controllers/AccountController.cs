namespace WebServer.GameStoreApplication.Controllers
{
    using Server.Http.Contracts;
    using ViewModels.Account;

    public class AccountController : BaseController
    {
        private const string RegisterView = @"account\register";
        private const string LoginView = @"account\login";
        private const string ProfileView = @"account\profile";

        public IHttpResponse Register()
        {
            return this.FileViewResponse(RegisterView);
        }

        public IHttpResponse Register(RegisterViewModel model)
        {
            // todo
            
            // Validate model

            // Create User in db

            // Save session
            
            // Redirect
            return this.FileViewResponse("/");
        }


    }
}
