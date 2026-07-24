using CookWizard.Domain.Recipes.Models;
using CookWizard.Domain.Recipes.Repository;
using CookWizard.Infrastructure.Common.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace CookWizard.Infrastructure.Recipes.Persistance;

public class MongoRecipeRepository : IRecipeRepository
{
    private readonly IMongoCollection<Recipe> _recipes;
    private readonly IMongoCollection<BsonDocument> _recipeDocuments;

    public MongoRecipeRepository(
        IMongoClient client,
        IOptions<MongoSettings> options
    )
    {
        var settings = options.Value;
        var database = client.GetDatabase("CookWizard");

        _recipes = database.GetCollection<Recipe>("Recipes");
        _recipeDocuments = database.GetCollection<BsonDocument>("Recipes");
    }

    public async Task<string> AddAsync(Recipe recipe)
    {
        await _recipes.InsertOneAsync(recipe);
        return recipe.Id;
    }

    public async Task<Recipe?> GetByIdAsync(string id)
    {
        return await _recipes.Find(r => r.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<RecipeSearchResult> SearchAsync(RecipeSearchCriteria criteria)
    {
        var itemsPipeline = BuildPipeline(criteria, includePagination: true);
        var countPipeline = BuildPipeline(criteria, includePagination: false);
        countPipeline.Add(new BsonDocument("$count", "totalItems"));

        var recipeDocuments = await _recipeDocuments.Aggregate<BsonDocument>(itemsPipeline).ToListAsync();
        var countDocuments = await _recipeDocuments.Aggregate<BsonDocument>(countPipeline).ToListAsync();

        var items = recipeDocuments
            .Select(document => BsonSerializer.Deserialize<Recipe>(document))
            .ToList();

        var totalItems = countDocuments.FirstOrDefault()?["totalItems"].ToInt64() ?? 0;

        return new RecipeSearchResult(items, totalItems);
    }

    private static List<BsonDocument> BuildPipeline(
        RecipeSearchCriteria criteria,
        bool includePagination
    )
    {
        var pipeline = new List<BsonDocument>();

        if (!string.IsNullOrWhiteSpace(criteria.SearchText))
        {
            pipeline.Add(new BsonDocument("$search", new BsonDocument
            {
                { "index", "default" },
                {
                    "text",
                    new BsonDocument
                    {
                        { "query", criteria.SearchText },
                        { "path", "name" },
                        { "fuzzy", new BsonDocument() }
                    }
                }
            }));
        }

        var match = BuildCookingTimeMatch(criteria);
        if (match.ElementCount > 0)
            pipeline.Add(new BsonDocument("$match", match));

        pipeline.Add(new BsonDocument("$sort", BuildSort(criteria.SortBy)));

        if (includePagination)
        {
            pipeline.Add(new BsonDocument("$skip", criteria.Skip));
            pipeline.Add(new BsonDocument("$limit", criteria.Limit));
        }

        return pipeline;
    }

    private static BsonDocument BuildCookingTimeMatch(RecipeSearchCriteria criteria)
    {
        var timeFilter = new BsonDocument();

        if (criteria.MinimumCookingTimeInSeconds.HasValue)
            timeFilter.Add("$gte", criteria.MinimumCookingTimeInSeconds.Value);

        if (criteria.MaximumCookingTimeInSeconds.HasValue)
            timeFilter.Add("$lte", criteria.MaximumCookingTimeInSeconds.Value);

        return timeFilter.ElementCount == 0
            ? new BsonDocument()
            : new BsonDocument("totalTimeInSeconds", timeFilter);
    }

    private static BsonDocument BuildSort(RecipeSearchSortBy sortBy)
    {
        return sortBy switch
        {
            RecipeSearchSortBy.Oldest => new BsonDocument("createdAt", 1),
            RecipeSearchSortBy.NameAscending => new BsonDocument("name", 1),
            RecipeSearchSortBy.NameDescending => new BsonDocument("name", -1),
            _ => new BsonDocument("createdAt", -1)
        };
    }
}
