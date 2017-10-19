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
        private const string DeleteGameView = @"admin\delete-game";
        private const string EditGameView = @"admin\edit-game";

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
                        .AppendLine($"<td>{g.Size:F1} GB</td>")
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

        public IHttpResponse DeleteView(int id)
        {
            // Accessed by Admins only
            if (!this.Authentication.IsAdmin)
            {
                return this.RedirectResponse(HomePath);
            }

            // Get Game from db
            var game = this.gameService.Find(id);
            if (game == null)
            {
                this.AddError($"Game #{id} does not exist");

                return this.List();
            }

            // Return View
            this.ViewData["id"] = game.Id.ToString();
            this.ViewData["title"] = game.Title;
            this.ViewData["description"] = game.Description;
            this.ViewData["image-url"] = game.ImageUrl;
            this.ViewData["price"] = game.Price.ToString("F2");
            this.ViewData["size"] = game.Size.ToString("F1");
            this.ViewData["video-id"] = game.VideoId;
            this.ViewData["release-date"] = game.ReleaseDate.ToString("yyyy-MM-dd");

            return this.FileViewResponse(DeleteGameView);
        }

        public IHttpResponse Delete(int id)
        {
            // Delete Game from db
            var success = this.gameService.Delete(id);
            if (!success)
            {
                this.AddError($"Game #{id} does not exist");
            }

            return this.List();
        }

        public IHttpResponse EditView(int id)
        {
            // Accessed by Admins only
            if (!this.Authentication.IsAdmin)
            {
                return this.RedirectResponse(HomePath);
            }

            // Get Game from db
            var game = this.gameService.Find(id);
            if (game == null)
            {
                this.AddError($"Game #{id} does not exist");

                return this.List();
            }

            // Return View
            this.ViewData["id"] = game.Id.ToString();
            this.ViewData["title"] = game.Title;
            this.ViewData["description"] = game.Description;
            this.ViewData["image-url"] = game.ImageUrl;
            this.ViewData["price"] = game.Price.ToString("F2");
            this.ViewData["size"] = game.Size.ToString("F1");
            this.ViewData["video-id"] = game.VideoId;
            this.ViewData["release-date"] = game.ReleaseDate.ToString("yyyy-MM-dd");

            return this.FileViewResponse(EditGameView);
        }

        public IHttpResponse Edit (AdminEditGameViewModel model)
        {
            // Accessed by Admins only
            if (!this.Authentication.IsAdmin)
            {
                return this.RedirectResponse(HomePath);
            }

            // Validate model
            if (!this.ValidateModel(model))
            {
                this.AddError("Invalid game details");

                return this.Edit(model);
            }

            // Update game in db
            var success = this.gameService.Update(
                                            model.Id,
                                            model.Title, 
                                            model.Description, 
                                            model.ImageUrl, 
                                            model.Price,
                                            model.Size, 
                                            model.VideoId, 
                                            (DateTime)model.ReleaseDate);

            if (!success)
            {
                this.AddError($"Game #{model.Id} does not exist");
            }

            return this.List();
        }


    }
}
