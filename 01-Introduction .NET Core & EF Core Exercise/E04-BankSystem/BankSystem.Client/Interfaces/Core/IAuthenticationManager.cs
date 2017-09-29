namespace BankSystem.Client.Interfaces.Core
{
    using BankSystem.Models;

    public interface IAuthenticationManager
    {
        bool IsAuthenticated();

        void Logout();

        void Login(User user);

        User GetCurrentUser();
    }
}
