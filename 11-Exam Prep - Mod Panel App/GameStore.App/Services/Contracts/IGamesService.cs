namespace GameStore.App.Services.Contracts
{
    using Data.Models;
    using Models.Games;
    using System;
    using System.Collections.Generic;

    public interface IGamesService
    {
        IEnumerable<GameListingAdminModel> AllGames();

        void Create(string title, string description, string thumbnail, 
            decimal price, double size, string videoId, DateTime releaseDate);

        Game GetById(int id);

        bool Update(int id, string title, string description, string thumbnail, 
            decimal price, double size, string videoId, DateTime releaseDate);

        bool Delete(int id);
    }
}
