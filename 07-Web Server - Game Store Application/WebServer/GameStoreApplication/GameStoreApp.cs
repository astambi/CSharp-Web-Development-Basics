namespace WebServer.GameStoreApplication
{
    using Controllers;
    using Data;
    using Microsoft.EntityFrameworkCore;
    using Server.Contracts;
    using Server.Routing.Contracts;
    using System;
    using System.Globalization;
    using ViewModels.Account;
    using ViewModels.Admin;

    public class GameStoreApp : IApplication
    {
        public void Configure(IAppRouteConfig appRouteConfig)
        {
            // Anonymous Paths
            appRouteConfig.AnonymousPaths.Add("/");
            appRouteConfig.AnonymousPaths.Add("/account/register");
            appRouteConfig.AnonymousPaths.Add("/account/login");

            // Routes
            appRouteConfig
                .Get(
                    "/", req => new HomeController(req).Index());

            appRouteConfig
                .Get(
                    "/account/register",
                    req => new AccountController(req).Register());

            appRouteConfig
               .Post(
                   "/account/register",
                   req => new AccountController(req).Register(
                       new RegisterViewModel
                       {
                           Email = req.FormData["email"],
                           FullName = req.FormData["full-name"],
                           Password = req.FormData["password"],
                           ConfirmPassword = req.FormData["confirm-password"],
                       }));

            appRouteConfig
                .Get(
                    "/account/login",
                    req => new AccountController(req).Login());

            appRouteConfig
                .Post(
                    "/account/login",
                    req => new AccountController(req).Login(
                        new LoginViewModel
                        {
                            Email = req.FormData["email"],
                            Password = req.FormData["password"]
                        }));

            appRouteConfig
                .Post(
                    "/account/logout",
                    req => new AccountController(req).Logout(req));

            appRouteConfig
                .Get(
                    "/admin/games/add",
                    req => new AdminController(req).Add());

            appRouteConfig
                .Post(
                    "/admin/games/add",
                    req => new AdminController(req).Add(
                        new AdminAddGameViewModel
                        {
                            Title = req.FormData["title"],
                            Description = req.FormData["description"],
                            ImageUrl = req.FormData["thumbnail"],
                            VideoId = req.FormData["video-id"],
                            ReleaseDate = DateTime.ParseExact(
                                          req.FormData["release-date"],
                                          "yyyy-MM-dd",
                                          CultureInfo.InvariantCulture),
                            Price = decimal.Parse(req.FormData["price"]),
                            Size = double.Parse(req.FormData["size"]),
                        }));

            appRouteConfig
                .Get(
                    "/admin/games/list",
                    req => new AdminController(req).List());

            appRouteConfig
                .Get(
                    "/admin/games/delete/{(?<id>[0-9]+)}",
                    req => new AdminController(req).DeleteView(
                        int.Parse(req.UrlParameters["id"])));

            appRouteConfig
                .Post(
                    "/admin/games/delete/{(?<id>[0-9]+)}",
                    req => new AdminController(req).Delete(
                        int.Parse(req.UrlParameters["id"])));

            appRouteConfig
                .Get(
                    "/admin/games/edit/{(?<id>[0-9]+)}",
                    req => new AdminController(req).EditView(
                        int.Parse(req.UrlParameters["id"])));

            appRouteConfig
                .Post(
                    "/admin/games/edit/{(?<id>[0-9]+)}",
                    req => new AdminController(req).Edit(
                        new AdminEditGameViewModel
                        {
                            Id = int.Parse(req.FormData["id"]),
                            Title = req.FormData["title"],
                            Description = req.FormData["description"],
                            ImageUrl = req.FormData["image-url"],
                            VideoId = req.FormData["video-id"],
                            ReleaseDate = DateTime.ParseExact(
                                          req.FormData["release-date"],
                                          "yyyy-MM-dd",
                                          CultureInfo.InvariantCulture),
                            Price = decimal.Parse(req.FormData["price"]),
                            Size = double.Parse(req.FormData["size"]),
                        }));
        }

        public void InitializeDatabase()
        {
            Console.WriteLine("Initializing database...");

            using (var context = new GameStoreDbContext())
            {
                context.Database.Migrate();
            }
        }
    }
}
