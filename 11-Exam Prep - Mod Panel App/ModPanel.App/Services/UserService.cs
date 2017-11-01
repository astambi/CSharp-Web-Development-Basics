namespace ModPanel.App.Services
{
    using Contracts;
    using Data;
    using Data.Models;
    using Models.Admin;
    using System.Collections.Generic;
    using System.Linq;

    public class UserService : IUserService
    {
        private readonly ModPanelDbContext context;

        public UserService(ModPanelDbContext context)
        {
            this.context = context;
        }

        public bool Create(string email, string password, PositionType position)
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
                Position = position,
                IsAdmin = isAdmin,
                IsApproved = isAdmin // assuming admin user are authomaticaly approved ;)
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

        public bool UserIsApproved(string email)
        {
            return this.context
                   .Users
                   .Any(u => u.Email == email && u.IsApproved);
        }

        public IEnumerable<AdminUserListingModel> All()
        {
            return this.context
                .Users
                .Select(u => new AdminUserListingModel
                {
                    Id = u.Id,
                    Email = u.Email,
                    Position = u.Position,
                    IsApproved = u.IsApproved,
                    Posts = u.Posts.Count
                })
                .ToList();
        }

        public string Approve(int id)
        {
            var user = this.context.Users.FirstOrDefault(u => u.Id == id);

            user.IsApproved = true;

            this.context.Users.Update(user);
            this.context.SaveChanges();

            return user.Email;
        }
    }
}
