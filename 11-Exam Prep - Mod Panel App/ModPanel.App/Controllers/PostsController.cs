namespace ModPanel.App.Controllers
{
    using Data.Models;
    using Models.Posts;
    using Services.Contracts;
    using SimpleMvc.Framework.Attributes.Methods;
    using SimpleMvc.Framework.Contracts;

    public class PostsController : BaseController
    {
        private readonly IPostService postService;

        public PostsController(IPostService postService)
        {
            this.postService = postService;
        }

        public IActionResult Create()
        {
            // Authenticate users
            if (!this.User.IsAuthenticated)
            {
                return this.RedirectToLogin();
            }

            return this.View();
        }

        [HttpPost]
        public IActionResult Create(PostModel model)
        {
            // Authenticate users
            if (!this.User.IsAuthenticated)
            {
                return this.RedirectToLogin();
            }

            // Validate model
            if (!this.IsValidModel(model))
            {
                this.ShowError(PostError);
                return this.View();
            }

            // Create post in db
            this.postService.Create(
                            model.Title,
                            model.Content,
                            this.Profile.Id);

            // Log Admin activity
            if (this.IsAdmin)
            {
                this.Log(LogType.CreatePost, model.Title);
            }

            return this.RedirectToHome();
        }
    }
}
