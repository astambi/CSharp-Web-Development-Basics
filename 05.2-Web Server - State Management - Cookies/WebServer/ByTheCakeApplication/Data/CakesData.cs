namespace WebServer.ByTheCakeApplication.Data
{
    using Models;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class CakesData
    {
        private const string DataBaseFile = @"ByTheCakeApplication\Data\database.csv";
        private const char Delimiter = ',';

        public IEnumerable<Cake> All()
        {
            return File
                .ReadAllLines(DataBaseFile)
                .Where(l => l.Contains(Delimiter))
                .Select(l => l.Split(Delimiter))
                .Select(l => new Cake
                {
                    Id = int.Parse(l[0]),
                    Name = l[1],
                    Price = decimal.Parse(l[2])
                });
        }

        public void Add(string name, string price)
        {
            var streamReader = new StreamReader(DataBaseFile);
            var id = streamReader.ReadToEnd().Split(Environment.NewLine).Length;
            streamReader.Dispose();

            using (var streamWriter = new StreamWriter(DataBaseFile, true))
            {
                streamWriter.WriteLine($"{id},{name},{price}");
            }
        }

        public Cake Find(int id)
        {
            return this.All().FirstOrDefault(c => c.Id == id);
        }
    }
}
