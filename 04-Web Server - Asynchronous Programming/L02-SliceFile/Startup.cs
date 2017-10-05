namespace L02_SliceFile
{
    using System;
    using System.IO;
    using System.Threading.Tasks;

    public class Startup
    {
        public static void Main()
        {
            Console.Write("Enter source path: ");
            var sourcePath = Console.ReadLine();

            Console.Write("Enter destination folder: ");
            var destinationPath = Console.ReadLine();

            Console.Write("Enter number of slices: ");
            var slices = int.Parse(Console.ReadLine());

            Task
                .Run(async () => await Slice(sourcePath, destinationPath, slices))
                .GetAwaiter()
                .GetResult();

            //Task.Run(async () => await AssembleFileFromSlices(destinationPath));

            while (true)
            {
                Console.ReadLine();
            }
        }

        private static async Task Slice(string sourcePath, string destinationPath, int slices)
        {
            slices = Math.Max(1, slices);

            if (Directory.Exists(destinationPath))
            {
                Directory.Delete(destinationPath, true);
            }

            if (!Directory.Exists(destinationPath))
            {
                Directory.CreateDirectory(destinationPath);
            }

            using (var source = new FileStream(sourcePath, FileMode.Open))
            {
                var fileInfo = new FileInfo(sourcePath);
                var sliceLength = (source.Length / slices) + 1;
                var currentByte = 0;

                Console.WriteLine($"Slicing file {fileInfo.FullName}...");

                for (int slice = 1; slice <= slices; slice++)
                {
                    var destinationFilePath = $"{destinationPath}/Slice-{slice}{fileInfo.Extension}";

                    using (var destination = new FileStream(destinationFilePath, FileMode.Create))
                    {
                        var buffer = new byte[1024];
                        while (currentByte <= sliceLength * slice)
                        {
                            var readBytes = await source.ReadAsync(buffer, 0, buffer.Length);
                            if (readBytes == 0)
                            {
                                break;
                            }

                            await destination.WriteAsync(buffer, 0, readBytes);
                            currentByte += readBytes;
                        }
                    }

                    Console.WriteLine($"Slice {slice}/{slices} ready.");
                }

                Console.WriteLine("Slicing complete.");
            }
        }

        //private static async Task AssembleFileFromSlices(string destinationPath)
        //{
        //    Console.WriteLine($"Assembling slices...");

        //    var directoryInfo = new DirectoryInfo(destinationPath);
        //    var slices = directoryInfo.GetFiles();

        //    if (!Directory.Exists("assembled"));
        //    {
        //        Directory.CreateDirectory("assembled");
        //    }
        //    var buffer = new byte[1024];
        //    using (var assembledFile = new FileStream($"assembled/assembledFile{slices[0].Extension}", FileMode.Create))
        //    {
        //        for (int i = 0; i < slices.Length; i++)
        //        {
        //            using (var reader = new FileStream(slices[i].FullName, FileMode.Open))
        //            {
        //                while (true)
        //                {
        //                    var readBytes = await reader.ReadAsync(buffer, 0, buffer.Length);
        //                    if (readBytes == 0)
        //                    {
        //                        break;
        //                    }

        //                    assembledFile.Write(buffer, 0, readBytes);
        //                }
        //            }
        //        }

        //        Console.WriteLine("Slices assembled.");
        //    }
        //}
    }
}