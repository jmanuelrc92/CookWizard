using CookWizard.Domain.Entities;

namespace CookWizard.Domain.Interfaces;

public interface IRecipeRepository
{
    Task<Guid> CreateAsync(Recipe recipe);
    Task<Recipe?> GetByIdAsync(Guid id);
    Task<IEnumerable<Recipe>> SearchByIngredientAsync(List<string> products);
    Task<(IEnumerable<Recipe> Items, long Total)> GetRecipes(int pageNumber, int pageSize);
}
