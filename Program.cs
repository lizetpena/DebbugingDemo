using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Net.Http;
using System.Linq;

namespace DebuggingDemo
{
    public class Person
    {
        public string Name { get; set; }
        public string City { get; set; }
    }


    class Program
    {
        static async Task<string> GetData()
        {
            HttpClient client = new HttpClient();
            string result = await client.GetStringAsync("http://microsoft.com");
            return result;
        }

        const int recursionDepth = 10;
        static void Main(string[] args)
        {       

            List<Task> tasks = new List<Task>();
            var files = Directory.GetFiles(@".\books");
            foreach (var file in files)
            { 
                var t2 = Task.Run(() => CheckData(file));
                tasks.Add(t2);
            }
            Task.WaitAll(tasks.ToArray());
        }

        private static int Method1()
        {
            try
            {
                string s = null;
                return s.Count();
            }
            catch
            {
                return 10;
            }
        }

        private static void DoIt()
        {
            DoRecursion(100);
        }

        static int Sum(int x, int y)
        {
            return x + y;
        }

        static int Square(int x)
        {
            return x * x;
        }

        private static void FindFactors()
        {
            int i = 10;
            for (i = -100; i < 100; i++)
            {

                //Console.WriteLine("it is now {0}", i);
                try
                {
                    bool isFactor = 100 / i == 0;

                    if (isFactor)
                    {
                        Console.WriteLine("{0} is a factor of 100", i);
                    }
                }
                catch { }
            }
        }

        public static bool IsFactorOf100(int val)
        {
            if (100 / val == 0)
            {
                return true;
            }
            return false;
        }

        static string someXml;
        public static void DoRecursion(int recursionCount)
        {
            someXml = File.ReadAllText("Customers.xml");
            if (recursionCount <= 0)
            {
                //Debugger.Break();
                return;
            }

            Console.WriteLine("This is iteration {0}", recursionCount);
            DoRecursion(recursionCount - 1);
        }

        public static async Task CheckData(string filename)
        {
            Console.WriteLine($"Checking {filename}");
            var sometext = File.ReadAllText(filename);
            Task<int> linesTask = Task.Run(() => CountLines(sometext));
            Task<int> wordsTask = Task.Run(() => CountWords(sometext));
            Task<int> lettersTask = Task.Run(() => CountLetters(sometext));

            await Task.WhenAll(linesTask, wordsTask, lettersTask);

            Console.WriteLine($"{filename} has {linesTask.Result} lines");
            Console.WriteLine($"{filename} has {wordsTask.Result} words");
            Console.WriteLine($"{filename} has {lettersTask.Result} letters");
        }

        private static int CountLines(string text)
        {
            Console.WriteLine("Counting lines for " + text.Substring(0, 50));
            return CountDelimitedBy(text, '\n');
        }

        private static int CountWords(string text)
        {
            Console.WriteLine("Counting words for " + text.Substring(0, 50));
            return CountDelimitedBy(text, '\n');
        }

        private static int CountLetters(string text)
        {
            Console.WriteLine("Counting stuff for " + text.Substring(0, 50));
            return CountDelimitedBy(text, 't');
        }


        private static uint data = 0;

        private static int CountDelimitedBy(string text, char delimiter)
        {
            int count = 0;
            try
            {
                int index = 0;
                bool matching = false;
                while (index < text.Length)
                {
                    // ridiculous slow way to get first char
                    string substr = text.Substring(0, index + 1);
                    char c = substr.Last();

                    // fast way to get first char
                    //char c = substr[index];

                    if (matching && c == delimiter)
                    {
                        matching = false;
                    }
                    else if (!matching && c != delimiter)
                    {
                        matching = true;
                        count++;
                    }
                    index++;

                    if (index % 10000 == 0)
                        Console.WriteLine(index);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return count;
        }

        public static void VisualizerExample()
        {
            string someXml = File.ReadAllText("Customers.xml");
            throw new NotImplementedException("Havent built it yet!");
        }
    }
}
