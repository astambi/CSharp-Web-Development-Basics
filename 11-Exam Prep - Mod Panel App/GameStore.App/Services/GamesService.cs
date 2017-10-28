namespace GameStore.App.Services
{
    using Data;
    using Data.Models;
    using Models.Games;
    using Services.Contracts;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class GamesService : IGamesService
    {
        public IEnumerable<GameListingAdminModel> AllGames()
        {
            using (var context = new GameStoreDbContext())
            {
                return context
                    .Games
                    .Select(g => new GameListingAdminModel
                    {
                        Id = g.Id,
                        Name = g.Title,
                        Size = g.Size,
                        Price = g.Price
                    })
                    .ToList();
            }
        }

        public Game GetById(int id)
        {
            using (var context = new GameStoreDbContext())
            {
                return context
                    .Games
                    .FirstOrDefault(g => g.Id == id);
            }
        }

        public void Create(
            string title, string description, string thumbnail,
            decimal price, double size, string videoId, DateTime releaseDate)
        {
            using (var context = new GameStoreDbContext())
            {
                var game = new Game
                {
                    Title = title,
                    Description = description,
                    ThumbnailUrl = thumbnail,
                    Price = price,
                    Size = size,
                    VideoId = videoId,
                    ReleaseDate = releaseDate
                };

                context.Games.Add(game);
                context.SaveChanges();
            }
        }

        public bool Delete(int id)
        {
            using (var context = new GameStoreDbContext())
            {
                // Find game
                var game = this.GetById(id);
                if (game == null)
                {
                    return false;
                }
              
                context.Remove(game);
                context.SaveChanges();

                return true;
            }
        }

        public bool Update(
            int id, string title, string description, string thumbnail,
            decimal price, double size, string videoId, DateTime releaseDate)
        {
            using (var context = new GameStoreDbContext())
            {
                // Find game
                var game = this.GetById(id);
                if (game == null)
                {
                    return false;
                }

                game.Title = title;
                game.Description = description;
                game.ThumbnailUrl = thumbnail;
                game.Price = price;
                game.Size = size;
                game.VideoId = videoId;
                game.ReleaseDate = releaseDate;

                context.Update(game);
                context.SaveChanges();

                return true;
            }
        }

    }
}
