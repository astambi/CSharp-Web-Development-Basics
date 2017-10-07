namespace URLDecode
{
    using System;
    using System.Net;

    public class Startup
    {
        public static void Main()
        {
            var urlInput = Console.ReadLine();
            var decodedUrl = WebUtility.UrlDecode(urlInput);
            Console.WriteLine(decodedUrl);
        }
    }
}
