
namespace CookWizard.Application.Recipes.CreateRecipe;

public record CreateRecipeCommand(
    string Name,
    int Portions,
    int TotalTimeInSeconds,
    List<CreateSection> Sections
);

public record CreateSection(
    string Name,
    List<CreateIngredient> Ingredients,
    List<CreateStep> Steps
);

public record CreateIngredient(
    decimal Quantity,
    string Unit,
    string Product,
    string? Notes,
    string? Raw
);

public record CreateStep(string Description);