namespace CookWizard.Application.Recipes.UseCases.SearchRecipes;

public record RecipeSummaryDto(
    string Id,
    string Name,
    string? Thumbnail,
    int CookingTime,
    DateTime CreatedAt
);
