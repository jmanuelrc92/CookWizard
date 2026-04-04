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

    public async Task<(IEnumerable<Recipe> Items, long Total)> GetRecipes(int pageNumber, int pageSize)
    {
        var filter = Builders<Recipe>.Filter.Empty;
        var totalTask = _recipes.CountDocumentsAsync(filter);
        var itemTask = _recipes.Find(filter)
            .Skip((pageNumber - 1) * pageSize)
            .Limit(pageSize)
            .ToListAsync();
        await Task.WhenAll(totalTask, itemTask);

        return (itemTask.Result, totalTask.Result);
    }

    public async Task<IEnumerable<Recipe>> SearchByIngredientAsync(List<string> products)
    {
        // Filtro: Busca recetas donde al menos un ingrediente tenga un "Product" 
        // que esté en nuestra lista de productos proporcionada.
        var filter = Builders<Recipe>.Filter.ElemMatch(
            r => r.Ingredients,
            i => products.Contains(i.Product)
        );

        return await _recipes.Find(filter).ToListAsync();
    }
}
