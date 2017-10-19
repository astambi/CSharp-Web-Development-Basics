namespace WebServer.GameStoreApplication.Controllers
{
    using Server.Http.Contracts;
    using Services;
    using Services.Contracts;
    using System;
    using System.Text;
    using WebServer.GameStoreApplication.ViewModels.Home;

    public class HomeController : BaseController
    {
        private readonly IGameService gameService;

        public HomeController(IHttpRequest request)
            : base(request)
        {
            this.gameService = new GameService();
        }

        public IHttpResponse Index()
        {
            // Get games from db
            var games = this.gameService.AllGames();

            // Get games html
            var gamesHtml = new StringBuilder();

            for (int i = 0; i < games.Count; i += 3)
            {
                gamesHtml.AppendLine(@"<div class=""card-group"">");

                var maxGameCount = Math.Min(i + 3, games.Count);
                for (int j = i; j < maxGameCount; j++)
                {
                    var game = games[j];

                    gamesHtml
                        .AppendLine(@"<div class=""card col-4 thumbnail"">")
                        .AppendLine($@"<img
                                        class=""card-image-top img-fluid img-thumbnail""
                                        onerror = ""this.src='https://i.ytimg.com/vi/{game.VideoId}/maxresdefault.jpg';""
                                        src = ""https://i.ytimg.com/vi/{game.VideoId}/maxresdefault.jpg"">")
                        .AppendLine($@"<div class=""card-body"">
                                        <h4 class=""card-title"">{game.Title}</h4>
                                        <p class=""card-text""><strong>Price</strong> - {game.Price:f2}&euro;</p>
                                        <p class=""card-text""><strong>Size</strong> - {game.Size:f1} GB</p>
                                        <p class=""card-text"">{game.Description}</p>
                                        </div>")
                        .AppendLine($@"<div class=""card-footer"">
                                        <a class=""card-button btn btn-outline-primary"" name=""info"" href=""/games/{game.Id}"">Info</a>
                                        <a class=""card-button btn btn-primary"" name=""buy"" href=""/cart/{game.Id}"">Buy</a>
                                        </div>")
                        .AppendLine("</div>");

                }

                gamesHtml.AppendLine(@"<div class=""card-group"">");
            }

            this.ViewData["games"] = gamesHtml.ToString();

            return this.FileViewResponse(@"home\index");
        }





    }
}
