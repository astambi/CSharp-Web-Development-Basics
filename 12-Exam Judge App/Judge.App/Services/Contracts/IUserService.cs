namespace Judge.App.Services.Contracts
{
    using Data.Models;
    using System.Collections.Generic;

    public interface IUserService
    {
        bool Create(string email, string password, string fullName);

        bool UserExists(string email, string password);

        IEnumerable<User> All();
    }
}
