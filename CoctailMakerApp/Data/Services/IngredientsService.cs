using CoctailMakerApp.Data.Context;
using CoctailMakerApp.Data.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoctailMakerApp.Data.Services
{
    public class IngredientsService : DatabaseServiceBase
    {
        public Task<List<Ingredient>> LoadAll()
        {
            return Task.Run(() =>
            {
                using (var dbContext = new SqliteDbContext())
                {
                    return dbContext.Ingredients.ToList();
                }
            });
        }

        public Task Save(Ingredient ingredient)
        {
            return Task.Run(() =>
            {
                using (var dbContext = new SqliteDbContext())
                {
                    if (ingredient.Id != 0)
                    {
                        dbContext.Ingredients.Update(ingredient);
                    }
                    else
                    {
                        dbContext.Ingredients.Add(ingredient);
                    }
                    dbContext.SaveChanges();
                }
            });
        }

        public Task Delete(Ingredient ingredient)
        {
            return Task.Run(() =>
            {
                using (var dbContext = new SqliteDbContext())
                {
                    dbContext.Ingredients.Remove(ingredient);
                    dbContext.SaveChanges();
                }
            });
        }
    }
}
