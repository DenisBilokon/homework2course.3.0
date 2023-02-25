using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WordFrequencyReport
{
    class Program
    {
        static void Main(string[] args)
        {
            string directoryPath = @"C:\MyTextFiles";

            Func<string, IEnumerable<string>> tokenizer = text =>
            {
                var separators = new char[] { ' ', '.', ',', ':', ';', '!', '?', '\r', '\n' };
                return text.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            };

            Func<IEnumerable<string>, IDictionary<string, int>> wordFrequencyCounter = words =>
            {
                var frequencyCounter = new Dictionary<string, int>();
                foreach (var word in words)
                {
                    if (frequencyCounter.ContainsKey(word))
                    {
                        frequencyCounter[word]++;
                    }
                    else
                    {
                        frequencyCounter[word] = 1;
                    }
                }
                return frequencyCounter;
            };

            Action<IDictionary<string, int>> displayWordFrequency = frequencyCounter =>
            {
                foreach (var pair in frequencyCounter.OrderByDescending(pair => pair.Value))
                {
                    Console.WriteLine($"{pair.Key}: {pair.Value}");
                }
            };

            foreach (var filePath in Directory.GetFiles(directoryPath))
            {
                var fileContent = File.ReadAllText(filePath);
                var words = tokenizer(fileContent);
                var frequencyCounter = wordFrequencyCounter(words);

                Console.WriteLine($"Звіт з файлу '{Path.GetFileName(filePath)}':");
                displayWordFrequency(frequencyCounter);
                Console.WriteLine();
            }
        }
    }
}
