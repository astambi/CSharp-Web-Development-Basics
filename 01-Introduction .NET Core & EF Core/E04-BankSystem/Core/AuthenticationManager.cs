namespace BankSystem.Core
{
    using System;
    using Interfaces.Core;
    using Interfaces.Models;
    using BankSystem.Models;

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
                throw new InvalidOperationException("Cannot log out. No user is logged in.");
            }

            this.currentUser = null;
        }

        public void Login(User user)
        {
            if (IsAuthenticated())
            {
                throw new InvalidOperationException("You should logout first.");
            }

            this.currentUser = user ??
                               throw new InvalidOperationException("Incorrect username.");
        }

        public User GetCurrentUser()
        {
            return this.currentUser;
        }
    }
}
