namespace SimpleMvc.App.Controllers
{
    using BindingModels;
    using SimpleMvc.Data;
    using SimpleMvc.Domain;
    using SimpleMvc.Framework.Attributes.Methods;
    using SimpleMvc.Framework.Contracts;
    using SimpleMvc.Framework.Controllers;
    using System.Linq;

    public class UsersController : Controller
    {
        private const string HomePath = "/home/index";
        private const string LoginPath = "/users/login";
        private const string UsersAllPath = "/users/all";

        [HttpGet]
        public IActionResult Register()
        {
            // Authenticate user
            if (this.User.IsAuthenticated)
            {
                return Redirect(HomePath);
            }

            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterUserBindingModel model)
        {
            // Validate model
            if (!this.IsValidModel(model))
            {
                return View();
            }

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

            // SignIn user
            this.SignIn(model.Username);

            return Redirect(HomePath);
        }

        [HttpGet]
        public IActionResult Login()
        {
            // Authenticate user
            if (this.User.IsAuthenticated)
            {
                return Redirect(HomePath);
            }

            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginUserBindingModel model)
        {
            // Validate model
            if (!this.IsValidModel(model))
            {
                return View();
            }

            // Validate user credentials
            using (var context = new NotesDbContext())
            {
                var foundUser = context.Users
                    .Any(u => u.Username == model.Username &&
                              u.Password == model.Password);

                if (!foundUser)
                {
                    return Redirect(LoginPath);
                }
            }

            // SignIn user
            this.SignIn(model.Username);

            return Redirect(HomePath);
        }

        [HttpPost]
        public IActionResult Logout()
        {
            this.SignOut();

            return Redirect(HomePath);
        }

        [HttpGet]
        public IActionResult All()
        {
            // Authenticate user
            if (!this.User.IsAuthenticated)
            {
                return Redirect(LoginPath);
            }

            using (var context = new NotesDbContext())
            {
                // Get users from db
                var users = context
                    .Users
                    .Select(u => new
                    {
                        Id = u.Id,
                        Username = u.Username
                    })
                    .ToList();

                // Get users view data
                this.ViewModel["users"] =
                    users.Any()
                    ? string.Join(string.Empty,
                        users.Select(u => $"<li><a href=\"/users/profile?id={u.Id}\">{u.Username}</a></li>"))
                    : string.Empty;
            }

            return View();
        }

        [HttpGet]
        public IActionResult Profile(int id)
        {
            // Authenticate user
            if (!this.User.IsAuthenticated)
            {
                return Redirect(LoginPath);
            }

            using (var context = new NotesDbContext())
            {
                // Get user with notes from db
                var user = context
                    .Users
                    .Where(u => u.Id == id)
                    .Select(u => new
                    {
                        UserId = u.Id,
                        Username = u.Username,
                        Notes = u.Notes
                                .Select(n => new
                                {
                                    Title = n.Title,
                                    Content = n.Content
                                })
                                .ToList()
                    })
                    .FirstOrDefault();

                // Check if user exists
                if (user == null)
                {
                    return Redirect(UsersAllPath);
                }

                // Get user view data
                this.ViewModel["username"] = user.Username;
                this.ViewModel["userid"] = user.UserId.ToString();
                this.ViewModel["notes"] = string.Join(string.Empty,
                    user.Notes.Select(n => $"<li><strong>{n.Title}</strong> - {n.Content}</li>"));
            }

            return View();
        }

        [HttpPost]
        public IActionResult Profile(AddNoteBindingModel model)
        {
            // Validate model
            if (!this.IsValidModel(model))
            {
                return View();
            }

            using (var context = new NotesDbContext())
            {
                // Check if user exists
                var foundUser = context.Users.Any(u => u.Id == model.UserId);
                if (!foundUser)
                {
                    return Redirect(UsersAllPath);
                }

                // Add note to db
                var note = new Note
                {
                    Title = model.Title,
                    Content = model.Content,
                    OwnerId = model.UserId
                };

                context.Notes.Add(note);
                context.SaveChanges();
            }

            return Profile(model.UserId);
        }
    }
}
