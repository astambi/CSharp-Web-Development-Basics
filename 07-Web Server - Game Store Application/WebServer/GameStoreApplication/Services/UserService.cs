namespace WebServer.GameStoreApplication.Services
{
    using Contracts;
    using Data;
    using Data.Models;
    using System.Linq;

    public class UserService : IUserService
    {
        public bool Create(string email, string name, string password)
        {
            using (var context = new GameStoreDbContext())
            {
                // Ensure Unique Email
                var emailExists = context.Users.Any(u => u.Email == email);
                if (emailExists)
                {
                    return false;
                }

                // The first registered user is admin
                var isAdmin = !context.Users.Any();

                // Create User
                var user = new User
                {
                    Email = email,
                    Name = name,
                    Password = password,
                    IsAdmin = isAdmin
                };

                context.Add(user);
                context.SaveChanges();

                return true;
            }
        }

        public bool Find(string email, string password)
        {
            using (var context = new GameStoreDbContext())
            {
                return context.Users.Any(u => u.Email == email && u.Password == password);
            }
        }

        public bool IsAdmin(string email)
        {
            using (var context = new GameStoreDbContext())
            {
                return context.Users.Any(u => u.Email == email && 
                                              u.IsAdmin);
            }
        }
    }
}
