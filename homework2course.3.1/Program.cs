using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TransactionAnalyzer
{
    class Program
    {
        static void Main(string[] args)
        {
            
            string csvFilePath = "transactions.csv";
            string dateFormat = "dd/MM/yyyy";
            Func<string, DateTime> getDate = line => DateTime.ParseExact(line.Split(',')[0], dateFormat, null);
            Func<string, double> getAmount = line => double.Parse(line.Split(',')[1]);

            Action<DateTime, double> displayDailyTotal = (date, total) =>
            {
                Console.WriteLine($"За день {date.ToString(dateFormat)} було витрачено {total.ToString("C")}");
            };

            var transactionsByDate = new Dictionary<DateTime, List<double>>();
            int lineCount = 0;

            using (var reader = new StreamReader(csvFilePath))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var date = getDate(line);
                    var amount = getAmount(line);

                    if (!transactionsByDate.ContainsKey(date))
                    {
                        transactionsByDate[date] = new List<double>();
                    }

                    transactionsByDate[date].Add(amount);

                    lineCount++;

                    if (lineCount % 10 == 0)
                    {
                        SaveDataToFile(transactionsByDate, csvFilePath, dateFormat);

                        foreach (var entry in transactionsByDate)
                        {
                            displayDailyTotal(entry.Key, entry.Value.Sum());
                        }

                        transactionsByDate.Clear();
                    }
                }
            }

            SaveDataToFile(transactionsByDate, csvFilePath, dateFormat);

            foreach (var entry in transactionsByDate)
            {
                displayDailyTotal(entry.Key, entry.Value.Sum());
            }
        }

        static void SaveDataToFile(Dictionary<DateTime, List<double>> transactionsByDate, string csvFilePath, string dateFormat)
        {
            using (var writer = new StreamWriter(csvFilePath, true))
            {
                foreach (var entry in transactionsByDate)
                {
                    var date = entry.Key.ToString(dateFormat);
                    var total = entry.Value.Sum();
                    writer.WriteLine($"{date},{total}");
                }
            }
        }
    }
}
