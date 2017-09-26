namespace BankSystem
{
    using Core;
    using Data;
    using IO;
    using Utils;

    public class Startup
    {
        public static void Main()
        {
            var reader = new Reader();
            var writer = new Writer();

            var init = new Init(writer);

            var authenticationManager = new AuthenticationManager();
            var accountGenerator = new AccountGenerator();
            var commandProcessor = new CommandProcessor(authenticationManager, accountGenerator);

            var engine = new Engine(reader, writer, commandProcessor);

            init.InitializeDatabase();
            engine.Run();
        }
    }
}
