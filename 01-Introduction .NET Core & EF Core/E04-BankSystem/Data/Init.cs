namespace BankSystem.Data
{
    using Microsoft.EntityFrameworkCore;
    using Interfaces.Data;
    using Interfaces.IO;

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
                context.Database.EnsureCreated();

                //context.Database.Migrate();

                this.writer.WriteMessage("Database ready!");
            }
        }
    }
}
