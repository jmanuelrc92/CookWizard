using CookWizard.Domain.Entities;
using CookWizard.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace CookWizard.Infraestructure.Persistance;

public class MongoRecipeRepository : IRecipeRepository
{
    private readonly IMongoCollection<Recipe> _recipes;

    public MongoRecipeRepository(IConfiguration configuration)
    {
        var client = new MongoClient(configuration.GetConnectionString("MongoDb"));
        var database = client.GetDatabase("CookWizardDb");
        _recipes = database.GetCollection<Recipe>("Recipes");
    }

    public async Task<Guid> CreateAsync(Recipe recipe)
    {
        await _recipes.InsertOneAsync(recipe);
        return recipe.Id;
    }

    public async Task<Recipe?> GetByIdAsync(Guid id)
    {
        return await _recipes.Find(r => r.Id == id).FirstOrDefaultAsync();
    }
}
