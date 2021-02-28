using CoctailMakerApp.Data.Context;
using CoctailMakerApp.Data.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace CoctailMakerApp.Data.Services
{

    public class RecipeService : DatabaseServiceBase
    {
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

        private Task Save(Recipe recipe)
        {
            return Task.Run(() =>
            {
                using (var dbContext = new SqliteDbContext())
                {
                    dbContext.Recipes.Add(recipe);
                    dbContext.SaveChanges();
                }
            });
        }
    }
}
