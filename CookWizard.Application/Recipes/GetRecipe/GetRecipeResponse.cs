using CookWizard.Domain.Recipes.Models;

namespace CookWizard.Application.Recipes.GetRecipe;

public record GetRecipeResponse(
    string Id,
    string Name,
    int Portions,
    int TotalTimeInSeconds,
    string Difficulty,
    List<Section> Sections
);

public record GetRecipeSectionResponse(
    string Name,
    List<GetRecipeIngredientResponse> Ingredients,
    List<GetRecipeStepsResponse> Steps
);

public record GetRecipeIngredientResponse(
    string RawIngredient
);

public record GetRecipeStepsResponse(
    int Order,
    string Instructions
);