namespace BankSystem.Client.Utils
{
    using Microsoft.EntityFrameworkCore;
    using BankSystem.Client.Interfaces.IO;
    using BankSystem.Client.Interfaces.Utils;
    using BankSystem.Data;

    public class Init : IInit
    {
        private IWriter writer;

        public Init(IWriter writer)
        {
            this.writer = writer;
        }

        public void InitializeDatabase()
        {
            using (var context = new BankSystemDbContext())
            {
                this.writer.WriteMessage("Initializing database...");

                context.Database.EnsureDeleted();
                //context.Database.EnsureCreated();

                context.Database.Migrate();

                this.writer.WriteMessage("Database ready!");
            }
        }
    }
}
