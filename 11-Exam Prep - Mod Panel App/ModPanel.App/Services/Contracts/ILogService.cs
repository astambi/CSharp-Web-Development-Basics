namespace ModPanel.App.Services.Contracts
{
    using Data.Models;
    using Models.Log;
    using System.Collections.Generic;

    public interface ILogService
    {
        void Create(string admin, LogType type, string additionalInfo);

        IEnumerable<LogListingModel> All();
    }
}
