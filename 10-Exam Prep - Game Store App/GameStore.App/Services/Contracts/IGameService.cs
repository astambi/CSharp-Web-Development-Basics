namespace GameStore.App.Services.Contracts
{
    using Data.Models;
    using System;
    using System.Collections.Generic;

    public interface IGameService
    {
        IEnumerable<TModel> All<TModel>(int? userId);

        IEnumerable<TModel> ByIds<TModel>(IEnumerable<int> ids);

        Game GetById(int id);

        void Create(string title, string description, string thumbnail,
            decimal price, double size, string videoId, DateTime releaseDate);

        bool Update(int id, string title, string description, string thumbnail,
            decimal price, double size, string videoId, DateTime releaseDate);

        bool Delete(int id);

        bool Exists(int id);
    }
}
