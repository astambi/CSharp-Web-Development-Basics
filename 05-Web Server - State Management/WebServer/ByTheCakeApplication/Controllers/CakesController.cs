namespace WebServer.ByTheCakeApplication.Controllers
{
    using Infrastructure;
    using Models;
    using Server.Http.Contracts;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class CakesController : Controller
    {
        private const string DataBaseFileName = @"ByTheCakeApplication\Data\database.csv";
        private static readonly IList<Cake> cakes = new List<Cake>();

        public IHttpResponse Add()
        {
            var cakeValues = new Dictionary<string, string>
            {
                ["showResult"] = "none"
            };

            return this.FileViewResponse(@"cakes\add", cakeValues);
        }

        public IHttpResponse Add(string name, string price)
        {
            var cake = new Cake
            {
                Name = name,
                Price = decimal.Parse(price)
            };

            cakes.Add(cake);

            using (var streamWriter = new StreamWriter(DataBaseFileName, true))
            {
                streamWriter.WriteLine($"{name},{price}");
            }

            var cakeValues = new Dictionary<string, string>
            {
                ["name"] = name,
                ["price"] = price,
                ["display"] = "block"
            };

            return this.FileViewResponse(@"cakes\add", cakeValues);
        }

        public IHttpResponse Search(IDictionary<string, string> urlParameters)
        {
            const string SearchTermKey = "searchTerm";
            const char Delimiter = ',';

            var results = string.Empty;

            if (urlParameters.ContainsKey(SearchTermKey))
            {
                var searchTerm = urlParameters[SearchTermKey];

                var savedCakes = File
                    .ReadAllLines(DataBaseFileName)
                    .Where(l => l.Contains(Delimiter))
                    .Select(l => l.Split(Delimiter))
                    .Select(l => new Cake
                    {
                        Name = l[0],
                        Price = decimal.Parse(l[1])
                    })
                    .Where(c => c.Name.ToLower().Contains(searchTerm.ToLower()))
                    .OrderBy(c => c.Name)
                    .Select(c => $"<div>{c.Name} - ${c.Price:f2}</div>")
                    .ToList();

                results = string.Join(Environment.NewLine, savedCakes);
            }

            var cakeValues = new Dictionary<string, string>
            {
                ["results"] = results,
            };

            return this.FileViewResponse(@"cakes\search", cakeValues);
        }
    }
}
