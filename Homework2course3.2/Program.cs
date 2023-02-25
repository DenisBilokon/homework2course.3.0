using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        
        string directory = @"C:\products\";

       
        List<Predicate<Product>> filters = new List<Predicate<Product>>();

       
        filters.Add(p => p.Price < 10);
        filters.Add(p => p.Category == "Fruit"); 

     
        for (int i = 1; i <= 10; i++)
        {
            string filename = Path.Combine(directory, i.ToString() + ".json");
            if (File.Exists(filename))
            {
            
                string jsonString = File.ReadAllText(filename);
                List<Product> products = JsonSerializer.Deserialize<List<Product>>(jsonString);

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
