namespace BankSystem.Client
{
    using BankSystem.Client.Core;
    using BankSystem.Client.IO;
    using BankSystem.Client.Utils;

    public class Startup
    {
        public static void Main()
        {
            var reader = new Reader();
            var writer = new Writer();

            var authenticationManager = new AuthenticationManager();
            var commandProcessor = new CommandProcessor(authenticationManager);
            var engine = new Engine(reader, writer, commandProcessor);

            var init = new Init(writer);

            init.InitializeDatabase();

            engine.Run();
        }
    }
}
