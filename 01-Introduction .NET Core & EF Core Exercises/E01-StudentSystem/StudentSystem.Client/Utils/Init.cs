namespace StudentSystem.Client.Utils
{
    using Microsoft.EntityFrameworkCore;
    using System;
    using Data;

    public class Init
    {
        public void InitializeDatabase()
        {
            Console.WriteLine("Initializing database...");

            using (var context = new StudentSystemDbContext())
            {
                //context.Database.EnsureDeleted(); // for testing purposes only
                //context.Database.EnsureCreated(); // NB! creates a new db, if db exists => no action, cannot migrate db

                context.Database.Migrate();
            }

            Console.WriteLine("Database ready!");
        }
    }
}
