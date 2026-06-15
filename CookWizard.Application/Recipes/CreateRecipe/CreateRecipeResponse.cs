namespace CookWizard.Application.Recipes.CreateRecipe;

public record CreateRecipeResponse(
    string Id,
    string Name,
    int Portions,
    int TotalTimeInSeconds,
    DateTime CreatedAt
);