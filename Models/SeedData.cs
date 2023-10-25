using Microsoft.EntityFrameworkCore;
using Recipease.Data;

namespace Recipease.Models;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var context = new RecipeaseContext(
            serviceProvider.GetRequiredService<
                DbContextOptions<RecipeaseContext>>()))
        {
            if (context == null || context.Recipe == null)
            {
                throw new ArgumentNullException("Null RecipeaseContext");
            }

            // Look for any recipes.
            if (context.Recipe.Any())
            {
                return;   // DB has been seeded
            }

            context.Recipe.AddRange(
                new Recipe
                {
                    Title = "Beef Stew",
                    DateAdded = DateTime.Parse("2023-5-26"),
                    Cuisine = "Irish",
                    Price = 7.99M,
                    Rating = 80
                },

                new Recipe
                {
                    Title = "Pasta Bake",
                    DateAdded = DateTime.Parse("2023-5-25"),
                    Cuisine = "Australian",
                    Price = 8.99M,
                    Rating = 95
                },

                new Recipe
                {
                    Title = "Pumpkin Soup",
                    DateAdded = DateTime.Parse("2023-5-26"),
                    Cuisine = "Irish",
                    Price = 9.99M,
                    Rating = 50
                },

                new Recipe
                {
                    Title = "Chicken Black Bean",
                    DateAdded = DateTime.Parse("2023-5-20"),
                    Cuisine = "Chinese",
                    Price = 3.99M,
                    Rating = 68
                }
            );
            context.SaveChanges();
        }
    }
}