namespace CookWizard.Application.Recipes.UseCases.SearchRecipes;

public record SearchRecipesResponse(
    IReadOnlyCollection<RecipeSummaryDto> Items,
    int Page,
    int PageSize,
    long TotalItems,
    int TotalPages
);
