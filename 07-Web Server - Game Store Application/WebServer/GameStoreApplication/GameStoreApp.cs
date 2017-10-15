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

            appRouteConfig
                .Get(
                    "/account/register",
                    req => new AccountController().Register());

            appRouteConfig
               .Post(
                   "/account/register",
                   req => new AccountController().Register(
                       new RegisterViewModel
                       {
                           Email = req.FormData["email"],
                           FullName = req.FormData["full-name"],
                           Password = req.FormData["password"],
                           ConfirmPassword = req.FormData["confirm-password"],
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
