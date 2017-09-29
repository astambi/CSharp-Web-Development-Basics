namespace BankSystem.Client.IO
{
    using System;
    using Interfaces.IO;

    public class Reader : IReader
    {
        public string ReadLine()
        {
            return Console.ReadLine();
        }
    }
}
