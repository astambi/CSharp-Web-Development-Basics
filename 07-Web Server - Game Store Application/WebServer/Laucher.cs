namespace WebServer
{
    using Application;
    using ByTheCakeApplication;
    using GameStoreApplication;

    using Server;
    using Server.Contracts;
    using Server.Routing;

    public class Launcher : IRunnable
    {
        private const int Port = 1337;

        public static void Main()
        {
            new Launcher().Run();
        }

        public void Run()
        {
            //var mainApplication = new MainApplication();

            //var mainApplication = new ByTheCakeApp();

            var mainApplication = new GameStoreApp();

            mainApplication.InitializeDatabase();

            var appRouteConfig = new AppRouteConfig();
            mainApplication.Configure(appRouteConfig);

            var webServer = new WebServer(Port, appRouteConfig);
            webServer.Run();
        }
    }
}
