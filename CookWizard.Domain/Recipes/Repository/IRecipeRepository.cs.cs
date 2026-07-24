using CookWizard.Domain.Recipes.Models;

namespace CookWizard.Domain.Recipes.Repository;

public interface IRecipeRepository
{
    Task<string> AddAsync(Recipe recipe);
    Task<Recipe?> GetByIdAsync(string id);
    Task<RecipeSearchResult> SearchAsync(RecipeSearchCriteria criteria);
}
