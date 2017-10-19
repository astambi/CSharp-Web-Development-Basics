namespace WebServer.GameStoreApplication.Services
{
    using Data;
    using Data.Models;
    using Services.Contracts;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ViewModels.Admin;

    class GameService : IGameService
    {
        public void Create(
                    string title,
                    string description,
                    string image,
                    decimal price,
                    double size,
                    string videoId,
                    DateTime releaseDate)
        {
            using (var context = new GameStoreDbContext())
            {
                var game = new Game
                {
                    Title = title,
                    Description = description,
                    ImageUrl = image,
                    Price = price,
                    Size = size,
                    VideoId = videoId,
                    ReleaseDate = releaseDate
                };

                context.Add(game);
                context.SaveChanges();
            }
        }

        public IEnumerable<AdminListGameViewModel> All()
        {
            using (var context = new GameStoreDbContext())
            {
                return context
                      .Games
                      .Select(g => new AdminListGameViewModel
                      {
                          Id = g.Id,
                          Name = g.Title,
                          Price = g.Price,
                          Size = g.Size
                      })
                      .ToList();
            }
        }
    }
}
