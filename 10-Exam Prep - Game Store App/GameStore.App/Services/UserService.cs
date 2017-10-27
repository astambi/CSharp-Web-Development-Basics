namespace GameStore.App.Services
{
    using Contracts;
    using Data;
    using Data.Models;
    using System.Linq;

    public class UserService : IUserService
    {
        public bool Create(string email, string password, string name)
        {
            using (var context = new GameStoreDbContext())
            {
                // Unique Email
                if (context.Users.Any(u => u.Email == email))
                {
                    return false;
                }

                // Create user
                var isAdmin = !context.Users.Any(); // first user is admin

                var user = new User
                {
                    Email = email,
                    Password = password,
                    FullName = name,
                    IsAdmin = isAdmin
                };

                context.Users.Add(user);
                context.SaveChanges();

                return true;
            }
        }

        public bool UserExists(string email, string password)
        {
            using (var context = new GameStoreDbContext())
            {
                return context
                    .Users
                    .Any(u => u.Email == email && u.Password == password);
            }
        }


    }
}
