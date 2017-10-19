namespace WebServer.GameStoreApplication.Services
{
    using Data;
    using Data.Models;
    using Services.Contracts;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ViewModels.Admin;
    using ViewModels.Home;

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

        public AdminDeleteGameViewModel Find(int id)
        {
            using (var context = new GameStoreDbContext())
            {
                return context
                        .Games
                        .Where(g => g.Id == id)
                        .Select(g => new AdminDeleteGameViewModel
                        {
                            Id = g.Id,
                            Title = g.Title,
                            Description = g.Description,
                            ImageUrl = g.ImageUrl,
                            Price = g.Price,
                            Size = g.Size,
                            VideoId = g.VideoId,
                            ReleaseDate = g.ReleaseDate

                        })
                        .FirstOrDefault();
            }
        }

        public bool Delete(int id)
        {
            using (var context = new GameStoreDbContext())
            {
                var game = context.Games.FirstOrDefault(g => g.Id == id);

                if (game == null)
                {
                    return false;
                }

                context.Remove(game);
                context.SaveChanges();

                return true;
            }
        }

        public bool Update(int id,
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
                var game = context.Games.FirstOrDefault(g => g.Id == id);
                if (game == null)
                {
                    return false;
                }

                game.Title = title;
                game.Description = description;
                game.ImageUrl = image;
                game.Price = price;
                game.Size = size;
                game.VideoId = videoId;
                game.ReleaseDate = releaseDate;

                context.Update(game);
                context.SaveChanges();

                return true;
            }
        }

        public IList<HomeUserGameListModel> AllGames()
        {
            using (var context = new GameStoreDbContext())
            {
                return context
                        .Games
                        .Select(g => new HomeUserGameListModel
                        {
                            Id = g.Id,
                            Title = g.Title,
                            Description = g.Description.Substring(0, 300),
                            Price = g.Price,
                            Size = g.Size,
                            VideoId = g.VideoId
                        })
                        .ToList();
            }
        }
    }
}
