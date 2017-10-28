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
        public bool Create(string email, string password, PositionType position)
        {
            using (var context = new ModPanelDbContext())
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
                    Position = position,
                    IsAdmin = isAdmin,
                    IsApproved = isAdmin // assuming admin user are authomaticaly approved ;)
                };

                context.Users.Add(user);
                context.SaveChanges();

                return true;
            }
        }

        public bool UserExists(string email, string password)
        {
            using (var context = new ModPanelDbContext())
            {
                return context
                       .Users
                       .Any(u => u.Email == email && u.Password == password);
            }
        }

        public bool UserIsApproved(string email)
        {
            using (var context = new ModPanelDbContext())
            {
                return context
                       .Users
                       .Any(u => u.Email == email && u.IsApproved);
            }
        }

        public IEnumerable<AdminUserListingModel> All()
        {
            using (var context = new ModPanelDbContext())
            {
                return context
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
        }

        public string Approve(int id)
        {
            using (var context = new ModPanelDbContext())
            {
                var user = context.Users.FirstOrDefault(u => u.Id == id);

                user.IsApproved = true;

                context.Users.Update(user);
                context.SaveChanges();

                return user.Email;
            }
        }
    }
}
