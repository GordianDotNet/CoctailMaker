using Blazored.Toast.Services;
using CoctailMakerApp.Data.Context;
using CoctailMakerApp.Data.Entities;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace CoctailMakerApp.Data.Services
{

    public class RecipeService : DatabaseServiceBase
    {
        public RecipeService(ILogger<RecipeService> logger) : base(logger)
        {
        }

        public Task<List<Recipe>> LoadAll()
        {
            return Task.Run(() =>
            {
                using (var dbContext = new SqliteDbContext())
                {
                    return dbContext.Recipes.ToList();
                }
            });
        }

        public Task Save(Recipe recipe)
        {
            return Task.Run(() =>
            {
                using (var dbContext = new SqliteDbContext())
                {
                    if (recipe.Id != 0)
                    {
                        dbContext.Recipes.Update(recipe);
                    }
                    else
                    {
                        dbContext.Recipes.Add(recipe);
                    }
                    dbContext.SaveChanges();
                }
            });
        }

        public Task Delete(Recipe recipe)
        {
            return Task.Run(() =>
            {
                using (var dbContext = new SqliteDbContext())
                {
                    dbContext.Recipes.Remove(recipe);
                    dbContext.SaveChanges();
                    //ToastService.ShowSuccess($"{nameof(Ingredient)}: [{ingredient.Id}] '{ingredient.Name}' [Deleted]");
                }
            });
        }
    }
}
