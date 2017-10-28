namespace GameStore.App
{
    using Data;
    using Microsoft.EntityFrameworkCore;
    using SimpleMvc.Framework;
    using SimpleMvc.Framework.Routers;
    using System;
    using WebServer;

    public class Launcher
    {
        static Launcher()
        {
            Console.WriteLine("Initializing database...");

            using (var context = new GameStoreDbContext())
            {
                context.Database.Migrate();
            }
        }


        public static void Main()
        {
            MvcEngine.Run(
                new WebServer(1337, new ControllerRouter(), new ResourceRouter()));
        }
    }
}
