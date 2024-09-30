using System;
using System.Text.Json;
using Core.Entities;

namespace Infrastructure.Data;

public class SeedData
{
    public static async Task SeedAysnc(AppDbContext context)
    {
        if (!context.Products.Any())
        {
            var productData = await File.ReadAllTextAsync("../Infrastructure/Data/SeedData/products.json");
            var products = JsonSerializer.Deserialize<List<Product>>(productData);
            if (products == null) return;
            context.Products.AddRange(products);
            await context.SaveChangesAsync();
        }
    }

}
