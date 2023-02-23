using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        // задаємо шлях до директорії з файлами JSON
        string directory = @"C:\products\";

        // створюємо список критеріїв для фільтрування продуктів
        List<Predicate<Product>> filters = new List<Predicate<Product>>();

        // додаємо критеріїв у список
        filters.Add(p => p.Price < 10); // продукти з ціною менше 10
        filters.Add(p => p.Category == "Fruit"); // фрукти

        // читаємо файли JSON з назвами від 1 до 10
        for (int i = 1; i <= 10; i++)
        {
            string filename = Path.Combine(directory, i.ToString() + ".json");
            if (File.Exists(filename))
            {
                // відкриваємо файл та десеріалізуємо його у список продуктів
                string jsonString = File.ReadAllText(filename);
                List<Product> products = JsonSerializer.Deserialize<List<Product>>(jsonString);

                // фільтруємо продукти за критеріями зі списку filters та виконуємо дії над відфільтрованими продуктами
                products.Where(p => filters.All(f => f(p))).ToList().ForEach(p => Console.WriteLine(p));
            }
        }
    }
}

class Product
{
    public string Name { get; set; }
    public string Category { get; set; }
    public decimal Price { get; set; }

    public override string ToString()
    {
        return $"{Name} ({Category}, {Price:C})";
    }
}
