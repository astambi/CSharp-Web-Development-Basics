namespace Demo
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    public class Startup
    {
        private static string result;

        public static void Main()
        {
            //Threads();
            //Lock();
            //Exceptions();
            //SlowTask();
            //Tasks(); 

            //AsyncAwait();

            //HttpClient
            //WebClient


            //Task
            //    .Run(async () =>
            //    {
            //        await DownloadFileAsync();
            //    })
            //    .GetAwaiter()
            //    .GetResult();


            //Task
            //    .Run(async () =>
            //    {
            //        var httpsClient = new HttpClient();
            //        var result = await httpsClient.GetStringAsync("http://www.mediapool.bg/");

            //        Console.WriteLine(result);
            //    })
            //    .GetAwaiter()
            //    .GetResult();


            //Task
            //    .Run(async () =>
            //    {
            //        //await GetAsync("http://mediapool.bg");
            //        await PostAsync("http://mediapool.bg");
            //    })
            //    .GetAwaiter()
            //    .GetResult();


            //var text = "Hello, world!";
            //var bytes = Encoding.UTF8.GetBytes(text);

            //foreach (var b in bytes)
            //{
            //    Console.WriteLine(b);
            //}

            //var textBack = Encoding.UTF8.GetString(bytes);
            //Console.WriteLine(textBack);


            Task
                .Run(async () =>
                {
                    using (var reader = new StreamReader("mediapool.html"))
                    {
                        while (true)
                        {
                            var line = await reader.ReadLineAsync();
                            if (line == null)
                            {
                                break;
                            }

                            Console.WriteLine(line);
                        }
                    }
                })
                .GetAwaiter()
                .GetResult();
            
        }

        private static async Task PostAsync(string url)
        {
            using (var httpClient = new HttpClient())
            {
                var contentToSend = new StringContent("test");
                var response = await httpClient.PostAsync(url, contentToSend);

                if (response.IsSuccessStatusCode)
                {
                    var headers = response.Headers;

                    foreach (var header in headers)
                    {
                        Console.WriteLine($"{header.Key}: {string.Join(", ", header.Value)}");
                    }

                    var content = await response.Content.ReadAsStringAsync();
                    //Console.WriteLine(content);
                }
                else
                {
                    Console.WriteLine(response.StatusCode);
                }
            }
        }

        private static async Task GetAsync(string url)
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(url);
                var headers = response.Headers;

                foreach (var header in headers)
                {
                    Console.WriteLine($"{header.Key}: {string.Join(", ", header.Value)}");
                }

                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(content);
            }
        }

        private static async Task DownloadFileAsync()
        {
            var webClient = new WebClient();
            Console.WriteLine("Downloadimg...");

            await webClient.DownloadFileTaskAsync("http://www.mediapool.bg/", "mediapool.html");

            Console.WriteLine("Finished");
        }




        private static void AsyncAwait()
        {
            DoWork2();
        }

        private static async void DoWork2()
        {
            var tasks = new List<Task>();
            var results = new List<bool>();

            for (int i = 0; i < 10; i++)
            {
                tasks.Add(Task.Run(async () =>
                {
                    var result = await SlowMethod();
                    results.Add(result);
                }));
            }

            //Task.WaitAll(tasks.ToArray());          // blocks the current idle thread
            await Task.WhenAll(tasks.ToArray());      // releases the current thread

            Console.WriteLine("Finished");
        }

        private static async Task<bool> SlowMethod() // Task.Result
        {
            Thread.Sleep(1000);
            Console.WriteLine("result");

            return true;
        }




        private static void SlowTask()
        {
            Console.WriteLine("Calculating ...");
            Task.Run(() => Calculate());

            Console.WriteLine("Enter command:");
            while (true)
            {
                var input = Console.ReadLine();
                if (input == "show")
                {
                    if (result == null)
                    {
                        Console.WriteLine("Still calculating...");
                    }
                    else
                    {
                        Console.WriteLine($"Result is {result}");
                    }
                }

                if (input == "Exit")
                {
                    break;
                }
            }
        }

        private static void Calculate()
        {
            Thread.Sleep(10000);
            result = "42";
        }

        private static void Tasks()
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var directoryInfo = new DirectoryInfo(currentDirectory + @"\images");
            var files = directoryInfo.GetFiles();

            const string resultDir = "Result";

            if (Directory.Exists(resultDir))
            {
            }

            Directory.CreateDirectory(resultDir);

            var tasks = new List<Task>();

            foreach (var file in files)
            {
                var task = Task.Run(() =>
                {
                    var image = Image.FromFile(file.FullName + 1);
                    image.RotateFlip(RotateFlipType.RotateNoneFlipY);
                    image.Save($"{resultDir}\\flipped-{file.Name}");

                    Console.WriteLine($"Processed - {file.Name} - {file.Length}");
                });

                tasks.Add(task);
            }

            try
            {
                Task.WaitAll(tasks.ToArray());
            }
            catch (AggregateException ex)
            {
                foreach (var e in ex.InnerExceptions)
                {
                    Console.WriteLine(e.Message);
                }
            }

            Console.WriteLine("Finished");

            // Tasks
            var secondTask = Task.Run(() =>
            {
                return 100;
            });

            Console.WriteLine(secondTask.Result);

            var thirdTask = Task.Run(() =>
            {
                return 200;
            });

            var result = thirdTask.GetAwaiter().GetResult();
            Console.WriteLine(result);

        }

        private static void Exceptions()
        {
            var thread = new Thread(() => DoWork());
            thread.Start();
        }

        private static void DoWork()
        {
            try
            {
                throw new InvalidOperationException();
            }
            catch (Exception)
            {
                Console.WriteLine("Catched");
            }
        }

        private static void Lock()
        {
            var numbers = Enumerable.Range(0, 10000).ToList();

            for (int i = 0; i < 4; i++)
            {
                var thread = new Thread(() =>
                {
                    while (numbers.Count > 0)
                    {
                        lock (numbers)
                        {
                            if (numbers.Count == 0)
                            {
                                break;
                            }

                            numbers.RemoveAt(numbers.Count - 1);
                        }
                    }
                });

                thread.Start();
            }
        }

        private static void Threads()
        {
            var thread = new Thread(() => PrintNumbersSlowly());
            Console.WriteLine("Printing...");

            while (true)
            {
                var line = Console.ReadLine();
                Console.WriteLine(line);
                Console.WriteLine();

                if (line == "exit")
                {
                    break;
                }
            }

            thread.Join();
        }

        private static void PrintNumbersSlowly()
        {
            for (int i = 0; i < 1000; i++)
            {
                System.Threading.Thread.Sleep(1000);
                Console.WriteLine(i);
            }
        }
    }
}
