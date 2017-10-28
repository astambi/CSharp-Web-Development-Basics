namespace ModPanel.App.Services
{
    using Contracts;
    using Data;
    using Data.Models;
    using Models.Log;
    using System.Collections.Generic;
    using System.Linq;

    public class LogService : ILogService
    {
        public void Create(string admin, LogType type, string additionalInfo)
        {
            using (var context = new ModPanelDbContext())
            {
                var log = new Log
                {
                    Admin = admin,
                    Type = type,
                    AdditionalInformation = additionalInfo
                };

                context.Logs.Add(log);
                context.SaveChanges();
            }
        }

        public IEnumerable<LogListingModel> All()
        {
            using (var context = new ModPanelDbContext())
            {
                return context
                   .Logs
                   .OrderByDescending(l => l.Id) // DESC
                   .Select(l => new LogListingModel
                   {
                       Admin = l.Admin,
                       Type = l.Type,
                       AdditionalInfo = l.AdditionalInformation
                   })
                   .ToList();
            }
        }
    }
}
