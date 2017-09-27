namespace BankSystem.Client.Core
{
    using System;
    using Interfaces.Core;
    using IO;
    using Models;

    public class AuthenticationManager : IAuthenticationManager
    {
        private User currentUser;

        public bool IsAuthenticated()
        {
            return this.currentUser != null;
        }

        public void Logout()
        {
            if (!IsAuthenticated())
            {
                throw new InvalidOperationException(OutputMessages.UserCannotLogOut);
            }

            this.currentUser = null;
        }

        public void Login(User user)
        {
            if (IsAuthenticated())
            {
                throw new InvalidOperationException(OutputMessages.UserShouldLogOut);
            }

            this.currentUser = user ??
                throw new InvalidOperationException(OutputMessages.InvalidUsername);
        }

        public User GetCurrentUser()
        {
            return this.currentUser;
        }
    }
}
