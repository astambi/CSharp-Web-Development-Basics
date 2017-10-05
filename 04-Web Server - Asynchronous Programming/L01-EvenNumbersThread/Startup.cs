namespace L01_EvenNumbersThread
{
    using System;
    using System.Linq;
    using System.Threading;

    public class Startup
    {
        public static void Main()
        {
            var range = Console.ReadLine()
                        .Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(int.Parse)
                        .ToArray();
            var minNum = range[0];
            var maxNum = range[1];

            var evenNumbers = new Thread(() => PrintEvenNumbers(minNum, maxNum));
            evenNumbers.Start();
            evenNumbers.Join();
            Console.WriteLine("Thread finished work");
        }

        private static void PrintEvenNumbers(int min, int max)
        {
            if (min % 2 != 0)
            {
                min++;
            }

            for (int i = min; i <= max; i += 2)
            {
                Console.WriteLine(i);
            }
        }
    }
}
