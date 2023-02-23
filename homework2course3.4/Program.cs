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
            string directoryPath = @"C:\MyTextFiles"; // змінити на шлях до теки з текстовими файлами

            // Функція для токенізації тексту
            Func<string, IEnumerable<string>> tokenizer = text =>
            {
                var separators = new char[] { ' ', '.', ',', ':', ';', '!', '?', '\r', '\n' };
                return text.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            };

            // Функція для обчислення частоти слів
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

            // Функція для відображення статистики частоти слів
            Action<IDictionary<string, int>> displayWordFrequency = frequencyCounter =>
            {
                foreach (var pair in frequencyCounter.OrderByDescending(pair => pair.Value))
                {
                    Console.WriteLine($"{pair.Key}: {pair.Value}");
                }
            };

            // Зчитування текстових файлів та підрахунок частоти слів
            foreach (var filePath in Directory.GetFiles(directoryPath))
            {
                var fileContent = File.ReadAllText(filePath);
                var words = tokenizer(fileContent);
                var frequencyCounter = wordFrequencyCounter(words);

                // Виведення звіту
                Console.WriteLine($"Звіт з файлу '{Path.GetFileName(filePath)}':");
                displayWordFrequency(frequencyCounter);
                Console.WriteLine();
            }
        }
    }
}
