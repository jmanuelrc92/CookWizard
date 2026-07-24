namespace CookWizard.Application.Recipes.UseCases.SearchRecipes;

public record RecipeSearchFilter(
    string? SearchText,
    RecipeCookingTimeFilter? CookingTime
);
