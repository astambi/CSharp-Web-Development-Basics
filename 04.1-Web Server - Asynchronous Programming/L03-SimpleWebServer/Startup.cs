namespace L03_SimpleWebServer
{
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading.Tasks;

    public class Startup
    {
        public static void Main()
        {
            var port = 1337;
            var ipAddress = IPAddress.Parse("127.0.0.1");
            var tcpListerner = new TcpListener(ipAddress, port);
            tcpListerner.Start();
            Console.WriteLine("Server started.");

            Console.WriteLine($"Listening to TCP Client at {ipAddress}:{port}");
            Task
                .Run(async () => await Connect(tcpListerner))
                .GetAwaiter()
                .GetResult();
        }

        private static async Task Connect(TcpListener tcpListerner)
        {
            while (true)
            {
                // Connect
                Console.WriteLine("Waiting for client...");
                var client = await tcpListerner.AcceptTcpClientAsync();
                Console.WriteLine("Client connected.");

                // Read
                var buffer = new byte[1024];
                await client.GetStream().ReadAsync(buffer, 0, buffer.Length);
                var clientMessage = Encoding.UTF8.GetString(buffer);
                Console.WriteLine(clientMessage.Trim('\0'));

                // Write
                var dataText = new StringBuilder()
                            .AppendLine("HTTP/1.1 200 OK")
                            .AppendLine("Content-Type: text/plain")
                            .AppendLine()
                            .AppendLine("Hello from server!");
                byte[] data = Encoding.UTF8.GetBytes(dataText.ToString().Trim());
                await client.GetStream().WriteAsync(data, 0, data.Length);

                // Close connection
                Console.WriteLine("Closing connection.");
                client.Dispose();
            }
        }
    }
}
