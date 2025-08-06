using Bogus;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Data;

public static class DatabaseSeedService
{
    public static async Task SeedAsync(AppDbContext dbContext)
    {
       
      //  dbContext.Database.EnsureCreated();

        if(await dbContext.Products.AnyAsync())
        {
            return; // Database already seeded
        }

        var products = GenerateProducts(30);

        await dbContext.Products.AddRangeAsync(products);
        
        await dbContext.SaveChangesAsync();
    }

    private static List<Product> GenerateProducts(int count = 50)
    {
        return new Faker<Product>()
            .RuleFor(p => p.Id, f => Guid.NewGuid())
            .RuleFor(p => p.Name, f => f.Commerce.ProductName())
            .RuleFor(p => p.Price, f => decimal.Parse(f.Commerce.Price(1, 100)))
            .Generate(count);
    }
}
