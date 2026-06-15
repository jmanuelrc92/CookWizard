using CookWizard.Domain.Recipes.Models;
using CookWizard.Domain.Recipes.Repository;
using CookWizard.Infrastructure.Common.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CookWizard.Infrastructure.Recipes.Persistance;

public class MongoRecipeRepository : IRecipeRepository
{
    private readonly IMongoCollection<Recipe> _recipes;

    public MongoRecipeRepository(
        IMongoClient client,
        IOptions<MongoSettings> options
    )
    {
        var settings = options.Value;
        var database = client.GetDatabase("CookWizard");

        _recipes = database.GetCollection<Recipe>("Recipes");
    }

    public async Task<string> AddAsync(Recipe recipe)
    {
        await _recipes.InsertOneAsync(recipe);
        return recipe.Id;
    }
}
