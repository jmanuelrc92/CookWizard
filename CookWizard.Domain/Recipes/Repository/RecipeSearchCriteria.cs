namespace CookWizard.Domain.Recipes.Repository;

public record RecipeSearchCriteria(
    string? SearchText,
    int? MinimumCookingTimeInSeconds,
    int? MaximumCookingTimeInSeconds,
    RecipeSearchSortBy SortBy,
    int Skip,
    int Limit
);

public enum RecipeSearchSortBy
{
    Newest,
    Oldest,
    NameAscending,
    NameDescending
}
