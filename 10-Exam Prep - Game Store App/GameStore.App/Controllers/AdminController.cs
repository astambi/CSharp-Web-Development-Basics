namespace GameStore.App.Controllers
{
    using Data.Models;
    using Infrastructure;
    using Models.Games;
    using Services.Contracts;
    using SimpleMvc.Framework.Attributes.Methods;
    using SimpleMvc.Framework.Contracts;
    using System.Linq;

    public class AdminController : BaseController
    {
        private const string AdminGamesPath = "/admin/games";

        private const string GameError = @"<p>Check your form for errors!</p><p>Title has to begin with an uppercase letter and has length between 3 and 100 symbols.</p><p>Trailer must be exactly 11 characters.</p><p>Thumbnail should start with http:// or https://.</p><p>Description should be at least 20 symbols long.</p>";
        private const string GameNotFound = "The requested game #{0} does not exist in the dababase!";

        private readonly IGameService gamesService;

        public AdminController(IGameService gamesService)
        {
            this.gamesService = gamesService;
        }

        public IActionResult Games()
        {
            // Accessible to Admins only
            if (!this.IsAdmin)
            {
                return this.Redirect(HomePage);
            }

            // Get games from db
            var games = this.gamesService.All<GameListingAdminModel>(null);

            // Create games table
            var gamesRows = games.Select(g => g.ToHtml());

            this.ViewModel["games"] = string.Join(string.Empty, gamesRows);

            return this.View();
        }

        public IActionResult Add()
        {
            // Accessible to Admins only
            if (!this.IsAdmin)
            {
                return this.Redirect(HomePage);
            }

            return this.View();
        }

        [HttpPost]
        public IActionResult Add(GameAdminModel model)
        {
            // Accessible to Admins only
            if (!this.IsAdmin)
            {
                return this.Redirect(HomePage);
            }

            // Validate model
            if (!this.IsValidModel(model))
            {
                this.ShowError(GameError);
                return this.View();
            }

            // Create Game in db
            this.gamesService.Create(
                model.Title, model.Description, model.ThumbnailUrl,
                model.Price, model.Size, model.VideoId, model.ReleaseDate);

            return this.Redirect(AdminGamesPath);
        }

        public IActionResult Edit(int id)
        {
            // Accessible to Admins only
            if (!this.IsAdmin)
            {
                return this.Redirect(HomePage);
            }

            // Get game from db
            var game = this.gamesService.GetById(id);
            if (game == null)
            {
                this.ShowError(string.Format(GameNotFound, id));
                return this.Games();
            }

            // Prepare View model
            this.GetGameViewData(game);

            return this.View();
        }

        [HttpPost]
        public IActionResult Edit(int id, GameAdminModel model)
        {
            // Accessible to Admins only
            if (!this.IsAdmin)
            {
                return this.Redirect(HomePage);
            }

            // Validate model
            if (!this.IsValidModel(model))
            {
                this.ShowError(GameError);
                return this.View();
            }

            // Update game in db
            var result = this.gamesService.Update(
                id, model.Title, model.Description, model.ThumbnailUrl,
                model.Price, model.Size, model.VideoId, model.ReleaseDate);

            if (!result)
            {
                this.ShowError(string.Format(GameNotFound, id));
                return this.Games();
            }

            return this.Redirect(AdminGamesPath);
        }

        public IActionResult Delete(int id)
        {
            // Accessible to Admins only
            if (!this.IsAdmin)
            {
                return this.Redirect(HomePage);
            }

            // Get game from db
            var game = this.gamesService.GetById(id);
            if (game == null)
            {
                this.ShowError(string.Format(GameNotFound, id));
                return this.Games();
            }

            // Prepare View model
            this.GetGameViewData(game);
            this.ViewModel["id"] = id.ToString();

            return this.View();
        }

        [HttpPost]
        public IActionResult ConfirmDelete(int id)
        {
            // Accessible to Admins only
            if (!this.IsAdmin)
            {
                return this.Redirect(HomePage);
            }

            // Remove game from db
            var result = this.gamesService.Delete(id);

            if (!result)
            {
                this.ShowError(string.Format(GameNotFound, id));
                return this.Games();
            }

            return this.Redirect(AdminGamesPath);
        }

        private void GetGameViewData(Game game)
        {
            this.ViewModel["title"] = game.Title;
            this.ViewModel["description"] = game.Description;
            this.ViewModel["thumbnail"] = game.ThumbnailUrl;
            this.ViewModel["video-id"] = game.VideoId;
            this.ViewModel["price"] = game.Price.ToString("F2");
            this.ViewModel["size"] = game.Size.ToString("F1");
            this.ViewModel["release-date"] = game.ReleaseDate.ToString("yyyy-MM-dd");
        }
    }
}
