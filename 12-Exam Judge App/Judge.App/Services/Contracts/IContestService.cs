namespace Judge.App.Services.Contracts
{
    using Models.Contests;
    using System.Collections.Generic;

    public interface IContestService
    {
        void Create(string name, int userId);

        IEnumerable<ContestListingModel> All();

        IEnumerable<ContestDropDownModel> AllDropDown();

        ContestModel GetById(int id);

        bool Update(int id, string name);

        bool Delete(int id);

        bool IsOwner(int id, int userId);
    }
}
