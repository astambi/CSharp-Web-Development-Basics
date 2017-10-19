namespace WebServer.GameStoreApplication.Controllers
{
    using Server.Http.Contracts;
    using Services;
    using Services.Contracts;
    using System;
    using System.Linq;
    using System.Text;
    using ViewModels.Admin;

    public class AdminController : BaseController
    {
        private const string AddGameView = @"admin\add-game";
        private const string ListGamesView = @"admin\list-games";

        private readonly IGameService gameService;

        public AdminController(IHttpRequest request)
            : base(request)
        {
            this.gameService = new GameService();
        }

        public IHttpResponse Add()
        {
            // Accessed by Admins only
            if (!this.Authentication.IsAdmin)
            {
                return this.RedirectResponse(HomePath);
            }

            return this.FileViewResponse(AddGameView);
        }

        public IHttpResponse Add(AdminAddGameViewModel model)
        {
            // Accessed by Admins only
            if (!this.Authentication.IsAdmin)
            {
                return this.RedirectResponse(HomePath);
            }

            // Validate model
            if (!this.ValidateModel(model))
            {
                return this.Add();
            }

            // Create Game in db
            this.gameService.Create(
                            model.Title, model.Description, model.ImageUrl, model.Price,
                            model.Size, model.VideoId, (DateTime)model.ReleaseDate);

            return this.RedirectResponse("/admin/games/list");
        }

        public IHttpResponse List()
        {
            // Accessed by Admins only
            if (!this.Authentication.IsAdmin)
            {
                return this.RedirectResponse(HomePath);
            }

            // Games from db
            var games = this.gameService
                .All()
                .Select(g => new StringBuilder()
                    .AppendLine("<tr>")
                        .AppendLine($"<td>{g.Id}</td>")
                        .AppendLine($"<td>{g.Name}</td>")
                        .AppendLine($"<td>{g.Size:F2} GB</td>")
                        .AppendLine($"<td>{g.Price:F2} &euro;</td>")
                        .AppendLine($"<td>")
                            .AppendLine($@"<a class=""btn btn-warning"" href=""/admin/games/edit/{g.Id}"">Edit</a>")
                            .AppendLine($@"<a class=""btn btn-danger"" href=""/admin/games/delete/{g.Id}"">Delete</a>")
                        .AppendLine($"</td>")
                    .AppendLine("</tr>")
                    .ToString());

            var gamesHtml = string.Join(Environment.NewLine, games);

            this.ViewData["games"] = gamesHtml;

            return this.FileViewResponse(ListGamesView);
        }
    }
}
