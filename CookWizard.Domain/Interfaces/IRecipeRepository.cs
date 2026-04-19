using CookWizard.Domain.Entities;

namespace CookWizard.Domain.Interfaces;

public interface IRecipeRepository
{
    Task<string> CreateAsync(Recipe recipe);
    Task<Recipe?> GetByIdAsync(string id);
    Task<IEnumerable<Recipe>> SearchByIngredientAsync(List<string> products);
    Task<(IEnumerable<Recipe> Items, long Total)> GetRecipes(int pageNumber, int pageSize);
    Task UpdateAsync(Recipe recipe);
    Task DeleteAsync(string id);
}
