namespace SimpleMvc.App
{
    using Microsoft.EntityFrameworkCore;
    using SimpleMvc.Data;
    using SimpleMvc.Framework;
    using SimpleMvc.Framework.Routers;
    using System;
    using WebServer;

    public class Launcher
    {
        public static void Main()
        {
            InitializeDatabase();

            var server = new WebServer(8000, new ControllerRouter());
            MvcEngine.Run(server);
        }

        private static void InitializeDatabase()
        {
            Console.WriteLine("Initializing database...");

            using (var context = new NotesDbContext())
            {
                context.Database.Migrate();
            }
        }
    }
}
