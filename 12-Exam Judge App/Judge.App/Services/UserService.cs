namespace Judge.App.Services
{
    using Contracts;
    using Data;
    using Data.Models;
    using System.Collections.Generic;
    using System.Linq;

    public class UserService : IUserService
    {
        private readonly JudgeDbContext context;

        public UserService(JudgeDbContext context)
        {
            this.context = context;
        }

        public bool Create(string email, string password, string fullName)
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
                FullName = fullName,
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

        public IEnumerable<User> All()
        {
            return this.context
                .Users
                .Select(u => new User
                {
                    Id = u.Id,
                    Email = u.Email,
                    FullName = u.FullName,
                    IsAdmin = u.IsAdmin
                })
                .ToList();
        }
    }
}
