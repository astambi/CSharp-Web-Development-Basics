namespace GameStore.App.Services
{
    using AutoMapper.QueryableExtensions;
    using Data;
    using Data.Models;
    using Services.Contracts;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class GameService : IGameService
    {
        private readonly GameStoreDbContext context;

        public GameService(GameStoreDbContext context)
        {
            this.context = context;
        }

        public IEnumerable<TModel> All<TModel>(int? userId)
        {
            var query = this.context.Games.AsQueryable();

            if (userId != null)
            {
                query = query.Where(g => g.Users.Any(u => u.UserId == userId));
            }

            return query
                .ProjectTo<TModel>()
                .ToList();
        }

        public IEnumerable<TModel> ByIds<TModel>(IEnumerable<int> ids)
        {
            return this.context
                .Games
                .Where(g => ids.Contains(g.Id))
                .ProjectTo<TModel>()
                .ToList();
        }

        public Game GetById(int id)
        {
            return this.context
                .Games
                .FirstOrDefault(g => g.Id == id);
        }

        public void Create(
            string title, string description, string thumbnail,
            decimal price, double size, string videoId, DateTime releaseDate)
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

            this.context.Games.Add(game);
            this.context.SaveChanges();
        }

        public bool Delete(int id)
        {
            // Find game
            var game = this.GetById(id);
            if (game == null)
            {
                return false;
            }

            this.context.Remove(game);
            this.context.SaveChanges();

            return true;
        }

        public bool Update(
            int id, string title, string description, string thumbnail,
            decimal price, double size, string videoId, DateTime releaseDate)
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

            this.context.Update(game);
            this.context.SaveChanges();

            return true;
        }

        public bool Exists(int id)
            => this.context.Games.Any(g => g.Id == id);
    }
}
