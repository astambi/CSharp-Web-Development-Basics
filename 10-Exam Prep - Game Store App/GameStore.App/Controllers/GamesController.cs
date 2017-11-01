namespace GameStore.App.Controllers
{
    using Services.Contracts;
    using SimpleMvc.Framework.Contracts;

    public class GamesController : BaseController
    {
        private readonly IGameService gameService;

        public GamesController(IGameService gameService)
        {
            this.gameService = gameService;
        }

        public IActionResult Details(int id)
        {
            // Get game from db
            var game = this.gameService.GetById(id);

            // Prepare View
            this.ViewModel["id"] = game.Id.ToString();
            this.ViewModel["title"] = game.Title;
            this.ViewModel["video-id"] = game.VideoId;
            this.ViewModel["description"] = game.Description;
            this.ViewModel["price"] = game.Price.ToString("F2");
            this.ViewModel["size"] = game.Size.ToString("F1");
            this.ViewModel["release-date"] = game.ReleaseDate.ToShortDateString();

            // Admin buttons Edit/ Delete
            this.ViewModel["adminButtons"] = string.Empty;

            if (this.IsAdmin)
            {
                this.ViewModel["adminButtons"] = $@"
                    <a class=""btn btn-warning"" href=""/admin/edit?id={id}"">Edit</a>
                    <a class=""btn btn-danger"" href=""/admin/delete?id={id}"">Delete</a>";
            }

            return this.View();
        }
    }
}
