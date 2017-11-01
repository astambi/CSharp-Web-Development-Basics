namespace ModPanel.App.Controllers
{
    using Infrastructure;
    using Services.Contracts;
    using SimpleMvc.Framework.Contracts;
    using System;
    using System.Linq;

    public class HomeController : BaseController
    {
        private readonly IPostService postService;
        private readonly ILogService logService;

        public HomeController(
            IPostService postService,
            ILogService logService) 
        {
            this.postService = postService;
            this.logService = logService;
        }

        public IActionResult Index()
        {
            // Guest Users
            this.ViewModel["guestDisplay"] = "block";
            this.ViewModel["authenticated"] = "none";
            this.ViewModel["admin"] = "none";

            // Authenticated Users
            if (this.User.IsAuthenticated)
            {
                this.ViewModel["guestDisplay"] = "none";
                this.ViewModel["authenticated"] = "flex";

                // Get Search term
                string search = null;
                if (this.Request.UrlParameters.ContainsKey("search"))
                {
                    search = this.Request.UrlParameters["search"];
                }

                // Get posts from db
                var postsData = this.postService.AllWithDetails(search);

                // Prepare posts view
                var postsCards = postsData
                    .Select(p => $@"
                        <div class=""card border-primary mb-3"">
                            <div class=""card-body text-primary"">
                                <h4 class=""card-title"">{p.Title}</h4>
                                <p class=""card-text"">{p.Content}</p>
                            </div>
                            <div class=""card-footer bg-transparent text-right"">
                                <span class=""text-muted"">
                                    Created on {(p.CreatedOn ?? DateTime.UtcNow).ToShortDateString()}
                                    by <em><strong>{p.CreatedBy}</strong></em>
                                </span>
                            </div>
                        </div>")
                    .ToList();

                this.ViewModel["posts"] = postsCards.Any()
                                        ? string.Join(string.Empty, postsCards)
                                        : "<h2>No posts found!</h2>";
                // Admin User Logs
                if (this.IsAdmin)
                {
                    this.ViewModel["authenticated"] = "none";
                    this.ViewModel["admin"] = "flex";

                    var logsHtml = this.logService
                        .All()
                        .Select(l => l.ToHtml());

                    this.ViewModel["logs"] = string.Join(string.Empty, logsHtml);
                }
            }

            return this.View();
        }
    }
}
