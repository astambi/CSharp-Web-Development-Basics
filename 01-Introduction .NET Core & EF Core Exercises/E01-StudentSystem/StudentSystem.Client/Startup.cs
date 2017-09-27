namespace StudentSystem.Client
{
    using StudentSystem.Client.Core;
    using StudentSystem.Client.Utils;

    public class Startup
    {
        public static void Main()
        {
            var init = new Init();
            init.InitializeDatabase();

            var engine = new Engine();
            engine.Run();
        }
    }
}
