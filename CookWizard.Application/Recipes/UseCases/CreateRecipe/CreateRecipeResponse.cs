namespace CookWizard.Application.Recipes.UseCases.CreateRecipe;

public record CreateRecipeResponse(
    string Id,
    string Name,
    int Portions,
    int TotalTimeInSeconds,
    DateTime CreatedAt
);