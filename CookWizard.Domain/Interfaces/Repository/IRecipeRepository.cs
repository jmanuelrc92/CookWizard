using CookWizard.Domain.Entities;

namespace CookWizard.Domain.Interfaces.Repository;

public interface IRecipeRepository
{
    Task<string> CreateAsync(Recipe recipe);
    Task<Recipe?> GetByIdAsync(string id);
    Task<IEnumerable<Recipe>> SearchByIngredientAsync(List<string> ingredients);
    Task<(IEnumerable<Recipe> Items, long Total)> GetRecipes(int pageNumber, int pageSize);
    Task UpdateAsync(Recipe recipe);
    Task DeleteAsync(string id);
    Task<(IEnumerable<Recipe>, long)> SearchAsync(
        int page,
        int size,
        List<string>? ingredients,
        string? difficulty,
        int? maxTime,
        int? minPortions
    );
}
