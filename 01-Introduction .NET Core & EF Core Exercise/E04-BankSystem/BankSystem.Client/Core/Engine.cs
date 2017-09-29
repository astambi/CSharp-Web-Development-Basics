namespace BankSystem.Client.Core
{
    using System;
    using Interfaces.Core;
    using Interfaces.IO;

    public class Engine : IEngine
    {
        private IReader reader;
        private IWriter writer;
        private ICommandProcessor commandProcessor;

        public Engine(IReader reader, IWriter writer, 
                      ICommandProcessor commandProcessor)
        {
            this.reader = reader;
            this.writer = writer;
            this.commandProcessor = commandProcessor;
        }

        public void Run()
        {
            while (true)
            {
                try
                {
                    var input = this.reader.ReadLine();
                    var output = this.commandProcessor.ProcessCommand(input);
                    this.writer.WriteMessage(output);
                }
                catch (Exception e)
                {
                    this.writer.WriteMessage(e.Message);
                }
            }
        }
    }
}
