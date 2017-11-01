namespace GameStore.App.Controllers
{
    using Infrastructure;
    using Models.Home;
    using Services.Contracts;
    using SimpleMvc.Framework.Contracts;
    using System.Linq;
    using System.Text;

    public class HomeController : BaseController
    {
        private readonly IGameService gameService;

        public HomeController(IGameService gameService)
        {
            this.gameService = gameService;
        }

        public IActionResult Index()
        {
            // Search filter
            var ownedFilter = this.User.IsAuthenticated &&
                              this.Request.UrlParameters.ContainsKey("filter") &&
                              this.Request.UrlParameters["filter"] == "Owned";

            // Get games from db
            var games = this.gameService
                .All<GameListingHomeModel>(ownedFilter ? (int?)this.Profile.Id : null)
                .Select(g => g.ToHtml(this.User.IsAuthenticated && this.IsAdmin))
                .ToList();

            // Prepare HTML
            var gamesHtml = new StringBuilder();

            for (int i = 0; i < games.Count; i++)
            {
                if (i % 3 == 0)
                {
                    gamesHtml.Append(@"<div class=""card-group"">");
                }

                gamesHtml.Append(games[i]);

                if (i % 3 == 2 ||
                    i == games.Count - 1)
                {
                    gamesHtml.Append("</div>");
                }
            }

            this.ViewModel["games"] = gamesHtml.ToString();

            return this.View();
        }
    }
}
