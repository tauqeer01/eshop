using System;
using System.Text.Json;
using Core.Entities;
namespace Infrastructure.Data
{
    public class SeedData
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            try
            {
                if (!context.Products.Any())
                {
                    var productData = await File.ReadAllTextAsync("../Infrastructure/Data/SeedData/products.json");
                    var products = JsonSerializer.Deserialize<List<Product>>(productData);
                    if (products == null) return;
                    context.Products.AddRange(products);
                    await context.SaveChangesAsync();
                }

                if (!context.DeliveryMethods.Any())
                {
                    var dmData = await File.ReadAllTextAsync("../Infrastructure/Data/SeedData/delivery.json");
                    var methods = JsonSerializer.Deserialize<List<DeliveryMethod>>(dmData);
                    if (methods == null) return;
                    context.DeliveryMethods.AddRange(methods);
                    await context.SaveChangesAsync();
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error seeding data: {ex.Message}");
            }
        }
    }
}
