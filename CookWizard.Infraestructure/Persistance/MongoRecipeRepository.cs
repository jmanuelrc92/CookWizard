using CookWizard.Domain.Entities;
using CookWizard.Domain.Interfaces.Repository;
using CookWizard.Infrastructure.Common.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
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
        string connectionUri = $"mongodb+srv://{_options.User}:{_options.Password}@{_options.Server}/?appName={_options.AppName}";
        
        var settings = MongoClientSettings.FromConnectionString(connectionUri);
        settings.ServerApi = new ServerApi(ServerApiVersion.V1);
        
        var client = new MongoClient(settings);
        try
        {
            var result = client.GetDatabase("admin").RunCommand<BsonDocument>(new BsonDocument("ping", 1));
            // todo, report a correct ping
        }
        catch(Exception ex)
        {
            //Todo exception report
        }
        return client.GetDatabase("CookWizard");
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

    public async Task<(IEnumerable<Recipe>, long)> SearchAsync(
        int page,
        int size,
        List<string>? ingredients,
        string? difficulty,
        int? maxTime,
        int? minPortions
    )
    {
        var builder = Builders<Recipe>.Filter;
        var filter = builder.Empty;

        // Productos (nested: Sections -> Ingredients)
        if (ingredients != null && ingredients.Any())
        {
            var normalized = ingredients.Select(p => p.ToLower()).ToList();

            var productFilter = builder.ElemMatch(
                r => r.Sections,
                s => s.Ingredients.Any(i => normalized.Contains(i.Product))
            );

            filter &= productFilter;
        }

        // Difficulty
        if (!string.IsNullOrWhiteSpace(difficulty))
        {
            if (Enum.TryParse<Difficulty>(difficulty, true, out var diffEnum))
            {
                filter &= builder.Eq(r => r.Difficulty, diffEnum);
            }
        }

        // Tiempo
        if (maxTime.HasValue)
        {
            filter &= builder.Lte(r => r.TotalTimeInSeconds, maxTime.Value);
        }

        // Porciones
        if (minPortions.HasValue)
        {
            filter &= builder.Gte(r => r.Portions, minPortions.Value);
        }

        var totalTask = _recipes.CountDocumentsAsync(filter);

        var itemsTask = _recipes.Find(filter)
            .Skip((page - 1) * size)
            .Limit(size)
            .ToListAsync();

        await Task.WhenAll(totalTask, itemsTask);

        return (itemsTask.Result, totalTask.Result);
    }

}