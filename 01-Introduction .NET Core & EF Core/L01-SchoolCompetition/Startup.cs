namespace SchoolCompetition
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class Startup
    {
        public static void Main()
        {
            var categories = new Dictionary<string, SortedSet<string>>();
            var scores = new Dictionary<string, int>();

            ReadInput(categories, scores);
            PrintResult(categories, scores);
        }

        private static void PrintResult(
            Dictionary<string, SortedSet<string>> categories, Dictionary<string, int> scores)
        {
            var orderedStudents = scores
                                .OrderByDescending(s => s.Value)
                                .ThenBy(s => s.Key);

            var builder = new StringBuilder();
            foreach (var studentsKvp in orderedStudents)
            {
                var name = studentsKvp.Key;
                var score = studentsKvp.Value;
                var studentCategoriesText = string.Join(", ", categories[name]);

                builder.AppendLine($"{name}: {score} [{studentCategoriesText}]");
            }

            Console.WriteLine(builder.ToString().Trim());
        }

        private static void ReadInput(
            Dictionary<string, SortedSet<string>> categories, Dictionary<string, int> scores)
        {
            while (true)
            {
                var input = Console.ReadLine();
                if (input == "END")
                {
                    break;
                }

                var tokens = input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                var name = tokens[0];
                var category = tokens[1];
                var score = int.Parse(tokens[2]);

                if (!categories.ContainsKey(name))
                {
                    categories[name] = new SortedSet<string>();
                }

                if (!scores.ContainsKey(name))
                {
                    scores[name] = 0;
                }

                categories[name].Add(category);
                scores[name] += score;
            }
        }
    }
}
