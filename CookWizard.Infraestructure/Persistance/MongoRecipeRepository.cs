using CookWizard.Domain.Entities;
using CookWizard.Domain.Interfaces;
using CookWizard.Infrastructure.Common.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CookWizard.Infraestructure.Persistance;

public class MongoRecipeRepository : IRecipeRepository
{
    private readonly IMongoCollection<Recipe> _recipes;
    private readonly MongoSettings _options;

    public MongoRecipeRepository(IOptions<MongoSettings> options)
    {
        _options = options.Value;

        this._recipes = this._ConfigureServerConnection()
            .GetCollection<Recipe>("Recipes");
    }

    private IMongoDatabase _ConfigureServerConnection()
    {
        var connectionString = $"mongodb://{_options.User}:{_options.Password}@localhost:{_options.Port}/myDatabase";
        var client = new MongoClient(connectionString);
        
        return client.GetDatabase("CookWizardDb");
    }

    public async Task<string> CreateAsync(Recipe recipe)
    {
        await _recipes.InsertOneAsync(recipe);
        return recipe.Id;
    }

    public async Task<Recipe?> GetByIdAsync(string id)
    {
        return await _recipes.Find(r => r.Id == id).FirstOrDefaultAsync();
    }

    public async Task<(IEnumerable<Recipe> Items, long Total)> GetRecipes(int pageNumber, int pageSize)
    {
        if (pageNumber <= 0) pageNumber = 1;
        if (pageSize <= 0) pageSize = 10;

        var filter = Builders<Recipe>.Filter.Empty;

        var totalTask = _recipes.CountDocumentsAsync(filter);
        var itemsTask = _recipes.Find(filter)
            .Skip((pageNumber - 1) * pageSize)
            .Limit(pageSize)
            .ToListAsync();

        await Task.WhenAll(totalTask, itemsTask);

        return (itemsTask.Result, totalTask.Result);
    }

    public async Task<IEnumerable<Recipe>> SearchByIngredientAsync(List<string> products)
    {
        var normalized = products.Select(p => p.ToLower()).ToList();

        var filter = Builders<Recipe>.Filter.ElemMatch(
            r => r.Sections,
            s => s.Ingredients.Any(i => normalized.Contains(i.Product))
        );

        return await _recipes.Find(filter).ToListAsync();
    }

    public async Task UpdateAsync(Recipe recipe)
    {
        await _recipes.ReplaceOneAsync(r => r.Id == recipe.Id, recipe);
    }

    public async Task DeleteAsync(string id)
    {
        await _recipes.DeleteOneAsync(r => r.Id == id);
    }
}
