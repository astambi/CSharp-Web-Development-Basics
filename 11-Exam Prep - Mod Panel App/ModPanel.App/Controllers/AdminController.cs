namespace ModPanel.App.Controllers
{
    using Data.Models;
    using Infrastructure;
    using Models.Posts;
    using Services.Contracts;
    using SimpleMvc.Framework.Attributes.Methods;
    using SimpleMvc.Framework.Contracts;
    using System.Linq;

    public class AdminController : BaseController
    {
        private const string AdminLogsPath = "/admin/log";
        private const string AdminPostsPath = "/admin/posts";
        private const string AdminUsersPath = "/admin/users";

        private const string PostNotFound = "The requested post #{0} does not exist in the dababase!";

        private readonly IUserService userService;
        private readonly IPostService postService;
        private readonly ILogService logService;

        public AdminController(
            IUserService userService,
            IPostService postService,
            ILogService logService)
        {
            this.userService = userService;
            this.postService = postService;
            this.logService = logService;
        }

        public IActionResult Users()
        {
            // Accessible to Admins only
            if (!this.IsAdmin)
            {
                return this.RedirectToLogin();
            }

            // Prepare view 
            var usersRows = this.userService
                .All()
                .Select(u => u.ToHtml())
                .ToList();

            this.ViewModel["users"] = string.Join(string.Empty, usersRows);

            // Log Admin activity
            if (this.IsAdmin)
            {
                this.Log(LogType.OpenMenu, "Users");
            }

            return this.View();
        }

        public IActionResult Approve(int id)
        {
            // Accessible to Admins only
            if (!this.IsAdmin)
            {
                return this.RedirectToLogin();
            }

            // Update IsApproved status in db
            var userEmail = this.userService.Approve(id);

            // Log Admin activity
            if (this.IsAdmin)
            {
                this.Log(LogType.UserApproval, userEmail);
            }

            return this.Redirect(AdminUsersPath);
        }

        public IActionResult Posts()
        {
            // Accessible to Admins only
            if (!this.IsAdmin)
            {
                return this.RedirectToLogin();
            }

            // Prepare view 
            var postsTable = this.postService
                .All()
                .Select(p => p.ToHtml())
                .ToList();

            this.ViewModel["posts"] = string.Join(string.Empty, postsTable);

            // Log Admin activity
            if (this.IsAdmin)
            {
                this.Log(LogType.OpenMenu, "Posts");
            }

            return this.View();
        }

        public IActionResult Edit(int id)
        {
            // Accessible to Admins only
            if (!this.IsAdmin)
            {
                return this.RedirectToLogin();
            }

            // Get post from db
            var post = this.postService.GetById(id);

            // Post Not Found error
            if (post == null)
            {
                this.ShowError(string.Format(PostNotFound, id));
                return this.Posts();
            }

            // Prepare View 
            this.GetPostViewData(post);

            return this.View();
        }

        [HttpPost]
        public IActionResult Edit(int id, PostModel model)
        {
            // Accessible to Admins only
            if (!this.IsAdmin)
            {
                return this.RedirectToLogin();
            }

            // Validate model
            if (!this.IsValidModel(model))
            {
                this.ShowError(PostError);
                this.GetPostViewData(model); // update post view data
                return this.View();
            }

            // Update post in db
            var result = this.postService.Update(
                        id,
                        model.Title,
                        model.Content);

            // Not Found error
            if (!result)
            {
                this.ShowError(string.Format(PostNotFound, id));
                return this.Posts();
            }

            // Log Admin activity
            if (this.IsAdmin)
            {
                this.Log(LogType.EditPost, model.Title);
            }

            return this.Redirect(AdminPostsPath);
        }

        public IActionResult Delete(int id)
        {
            // Accessible to Admins only
            if (!this.IsAdmin)
            {
                return this.RedirectToLogin();
            }

            // Get post from db
            var post = this.postService.GetById(id);

            // Post Not Found error
            if (post == null)
            {
                this.ShowError(string.Format(PostNotFound, id));
                return this.Posts();
            }

            // Prepare View 
            this.GetPostViewData(post);

            // All the above same as Edit GET

            this.ViewModel["id"] = id.ToString();

            return this.View();
        }

        [HttpPost]
        public IActionResult ConfirmDelete(int id)
        {
            // Accessible to Admins only
            if (!this.IsAdmin)
            {
                return this.RedirectToLogin();
            }

            // Get post from db
            var post = this.postService.GetById(id);

            // Remove post from db
            var result = this.postService.Delete(id);

            // Not Found error
            if (!result)
            {
                this.ShowError(string.Format(PostNotFound, id));
                return this.Posts();
            }

            // Log Admin activity
            if (this.IsAdmin)
            {
                this.Log(LogType.DeletePost, post.Title);
            }

            return this.Redirect(AdminPostsPath);
        }

        public IActionResult Log()
        {
            // Accessible to Admins only
            if (!this.IsAdmin)
            {
                return this.RedirectToLogin();
            }

            // Get log from db & prepare view
            var logRows = this.logService
                .All()
                .Select(l => l.ToHtml())
                .ToList();

            this.ViewModel["logs"] = string.Join(string.Empty, logRows);

            // Log Admin activity
            if (this.IsAdmin)
            {
                this.Log(LogType.OpenMenu, "Log");
            }

            return this.View();
        }

        private void GetPostViewData(PostModel post)
        {
            this.ViewModel["title"] = post.Title;
            this.ViewModel["content"] = post.Content;
        }
    }
}
