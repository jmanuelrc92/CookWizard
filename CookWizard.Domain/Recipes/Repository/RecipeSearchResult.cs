using CookWizard.Domain.Recipes.Models;

namespace CookWizard.Domain.Recipes.Repository;

public record RecipeSearchResult(
    IReadOnlyCollection<Recipe> Items,
    long TotalItems
);
