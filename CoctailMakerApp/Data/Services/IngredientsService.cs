using Blazored.Toast.Services;
using CoctailMakerApp.Data.Context;
using CoctailMakerApp.Data.Entities;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoctailMakerApp.Data.Services
{
    public class IngredientsService : DatabaseServiceBase
    {
        public IngredientsService(ILogger<IngredientsService> logger) : base(logger)
        {
        }

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
                        //ToastService.ShowInfo($"{nameof(Ingredient)}: '{ingredient.Name}' [Changing]");
                    }
                    else
                    {
                        dbContext.Ingredients.Add(ingredient);
                        //ToastService.ShowInfo($"{nameof(Ingredient)}: [Adding]");
                    }
                    dbContext.SaveChanges();
                    //ToastService.ShowSuccess($"{nameof(Ingredient)}: [{ingredient.Id}] '{ingredient.Name}' [Saved]");
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
                    //ToastService.ShowSuccess($"{nameof(Ingredient)}: [{ingredient.Id}] '{ingredient.Name}' [Deleted]");
                }
            });
        }
    }
}
