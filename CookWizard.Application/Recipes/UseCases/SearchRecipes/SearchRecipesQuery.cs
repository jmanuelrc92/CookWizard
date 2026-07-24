namespace CookWizard.Application.Recipes.UseCases.SearchRecipes;

public record SearchRecipesQuery(
    PaginationRequest? Pagination,
    RecipeSearchFilter? Filter,
    RecipeSortBy SortBy = RecipeSortBy.Newest
);
