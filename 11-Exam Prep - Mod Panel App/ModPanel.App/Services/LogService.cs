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
        private readonly ModPanelDbContext context;

        public LogService(ModPanelDbContext context)
        {
            this.context = context;
        }

        public void Create(string admin, LogType type, string additionalInfo)
        {
            var log = new Log
            {
                Admin = admin,
                Type = type,
                AdditionalInformation = additionalInfo
            };

            this.context.Logs.Add(log);
            this.context.SaveChanges();
        }

        public IEnumerable<LogListingModel> All()
        {
            return this.context
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
