
using Core.Entities;
using Microsoft.AspNetCore.Identity;
using System.Reflection;
using System.Text.Json;

namespace Infrastructure.Data.SeedData
{
    public class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext context, UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any(x=>x.UserName == "admin@test.com"))
            {
                var user = new AppUser()
                {
                    UserName = "admin@test.com",
                    Email = "admin@test.com"
                };

                await userManager.CreateAsync(user, "Pa$$w0rd");
                await userManager.AddToRoleAsync(user, "Admin");
            }
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            if (!context.Products.Any())
            {
                var productsData = await File
                    .ReadAllTextAsync(path + @"/Data/SeedData/products.json");

                var products = JsonSerializer.Deserialize<List<Product>>(productsData);
                if (products is null) return;

                context.Products.AddRange(products);
                await context.SaveChangesAsync();
            }

            if (!context.DeliveryMethods.Any())
            {
                var dmData = await File
                    .ReadAllTextAsync(path + @"/Data/SeedData/delivery.json");
                var deliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(dmData);
                if (deliveryMethods is null) return;

                context.DeliveryMethods.AddRange(deliveryMethods);
                await context.SaveChangesAsync();
            }

        }
    }
}
