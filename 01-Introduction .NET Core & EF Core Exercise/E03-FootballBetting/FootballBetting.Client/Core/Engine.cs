namespace FootballBetting.Client.Core
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using Data;

    public class Engine
    {
        public void Run()
        {
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            using (var context = new FootballBookmakerSystemDbContext())
            {
                Console.WriteLine("Initializing database...");

                context.Database.EnsureDeleted();
                //context.Database.EnsureCreated();

                context.Database.Migrate();

                Console.WriteLine("Database ready!");
            }
        }
    }
}
