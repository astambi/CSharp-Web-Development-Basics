namespace WebServer.ByTheCakeApplication.Controllers
{
    using Infrastructure;
    using Server.Http.Contracts;
    using System.Collections.Generic;

    public class AccountController : Controller
    {
        public IHttpResponse Login()
        {
            var loginValues = new Dictionary<string, string>
            {
                ["result"] = string.Empty,
            };

            return this.FileViewResponse(@"account\login", loginValues);
        }

        public IHttpResponse Login(string username, string password)
        {
            const string LoginMessage = "Hi {0}, your password is {1}";
            var result = string.Format(LoginMessage, username, password);

            var loginValues = new Dictionary<string, string>
            {
                ["result"] = result,
            };

            return this.FileViewResponse(@"account\login", loginValues);
        }

        public IHttpResponse LoginToEmail()
        {
            var loginValues = new Dictionary<string, string>
            {
                ["result"] = string.Empty,
            };

            return this.FileViewResponse(@"account\login", loginValues);
        }

        public IHttpResponse LoginToEmail(string username, string password)
        {
            const string ValidUsername = "suAdmin";
            const string ValidPassword = "aDmInPw17";
            const string LoginMessageSuccess = "Hello {0}!";
            const string LoginMessageFailure = "Invalid username or password!";

            var result = LoginMessageFailure;
            if (username == ValidUsername && password == ValidPassword)
            {
                result = string.Format(LoginMessageSuccess, username);
            }

            var loginValues = new Dictionary<string, string>
            {
                ["result"] = result,
            };

            return this.FileViewResponse(@"account\sendEmail", loginValues);
        }

        public IHttpResponse SendEmail()
        {
            var emailValues = new Dictionary<string, string>
            {
                ["result"] = string.Empty,
            };

            return this.FileViewResponse(@"account\sendEmail", emailValues);
        }

        public IHttpResponse SendEmail(string recipient, string subject, string message)
        {
            // todo


            var emailValues = new Dictionary<string, string>
            {
                ["result"] = "TEst",
            };

            return this.FileViewResponse(@"account\sendEmail", emailValues);
        }
    }
}
