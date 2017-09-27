namespace BankSystem.Client.IO
{
    using System;
    using Interfaces.IO;

    public class Writer : IWriter
    {
        public void WriteMessage(string message)
        {
            Console.WriteLine(message);
        }
    }
}
