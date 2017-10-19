namespace WebServer.GameStoreApplication.Services.Contracts
{
    using System;
    using System.Collections.Generic;
    using ViewModels.Admin;
    using ViewModels.Home;

    public interface IGameService
    {
        void Create(string title,
                    string description,
                    string image,
                    decimal price,
                    double size,
                    string videoId,
                    DateTime releaseDate);

        IEnumerable<AdminListGameViewModel> All();

        AdminDeleteGameViewModel Find(int id);

        bool Delete(int id);

        bool Update(int id,
                    string title,
                    string description,
                    string image,
                    decimal price,
                    double size,
                    string videoId,
                    DateTime releaseDate);

        IList<HomeUserGameListModel> AllGames();
    }
}
