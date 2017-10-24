namespace SimpleMvc.App.Controllers
{
    using BindingModels;
    using SimpleMvc.Data;
    using SimpleMvc.Domain;
    using SimpleMvc.Framework.Attributes.Methods;
    using SimpleMvc.Framework.Contracts;
    using SimpleMvc.Framework.Contracts.Generic;
    using SimpleMvc.Framework.Controllers;
    using System.Collections.Generic;
    using System.Linq;
    using ViewModels;

    public class UsersController : Controller
    {
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterUserBindingModel model)
        {
            // Add user to db
            using (var context = new NotesDbContext())
            {
                var user = new User
                {
                    Username = model.Username,
                    Password = model.Password
                };

                context.Users.Add(user);
                context.SaveChanges();
            }

            return View();
        }

        [HttpGet]
        public IActionResult<AllUsernamesViewModel> All()
        {
            // Get users from db
            List<UsernameViewModel> usernames = null;

            using (var context = new NotesDbContext())
            {
                usernames = context
                            .Users
                            .Select(u => new UsernameViewModel
                            {
                                UserId = u.Id,
                                Username = u.Username
                            })
                            .ToList();
            }

            // Create view model
            var allUsernamesViewModel = new AllUsernamesViewModel
            {
                UsernamesWithIds = usernames
            };

            return View(allUsernamesViewModel);
        }

        [HttpGet]
        public IActionResult<UserProfileViewModel> Profile(int id)
        {
            // Get data from db
            UserProfileViewModel userViewModel = null;

            using (var context = new NotesDbContext())
            {
                userViewModel = context
                                .Users
                                .Where(u => u.Id == id)
                                .Select(u => new UserProfileViewModel
                                {
                                    UserId = u.Id,
                                    Username = u.Username,
                                    Notes = u.Notes
                                            .Select(n => new NoteViewModel
                                            {
                                                Title = n.Title,
                                                Content = n.Content
                                            })
                                            .ToList()
                                })
                                .FirstOrDefault();
            }

            return View(userViewModel);
        }

        [HttpPost]
        public IActionResult<UserProfileViewModel> Profile(AddNoteBindingModel model)
        {
            // Add note to db
            using (var context = new NotesDbContext())
            {
                var user = context.Users.Find(model.UserId);
                var note = new Note
                {
                    Title = model.Title,
                    Content = model.Content
                };

                user.Notes.Add(note);
                context.SaveChanges();
            }

            return Profile(model.UserId);
        }
    }
}
