namespace GameStore.App.Services
{
    using Contracts;
    using Data;
    using Data.Models;
    using System.Linq;

    public class UserService : IUserService
    {
        private readonly GameStoreDbContext context;

        public UserService(GameStoreDbContext context)
        {
            this.context = context;
        }

        public bool Create(string email, string password, string name)
        {
            // Unique Email
            if (this.context.Users.Any(u => u.Email == email))
            {
                return false;
            }

            // Create user
            var isAdmin = !this.context.Users.Any(); // first user is admin

            var user = new User
            {
                Email = email,
                Password = password,
                FullName = name,
                IsAdmin = isAdmin
            };

            this.context.Users.Add(user);
            this.context.SaveChanges();

            return true;
        }

        public bool UserExists(string email, string password)
        {
            return this.context
                .Users
                .Any(u => u.Email == email && u.Password == password);
        }
    }
}
