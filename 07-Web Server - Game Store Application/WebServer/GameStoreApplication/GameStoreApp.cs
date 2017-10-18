namespace WebServer.GameStoreApplication
{
    using Controllers;
    using Data;
    using Microsoft.EntityFrameworkCore;
    using Server.Contracts;
    using Server.Routing.Contracts;
    using System;
    using ViewModels.Account;

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
