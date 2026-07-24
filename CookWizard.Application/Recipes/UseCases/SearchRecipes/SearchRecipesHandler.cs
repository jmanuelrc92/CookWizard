using CookWizard.Domain.Recipes.Models;
using CookWizard.Domain.Recipes.Repository;

namespace CookWizard.Application.Recipes.UseCases.SearchRecipes;

public class SearchRecipesHandler
{
    public async Task<SearchRecipesResponse> Handle(
        SearchRecipesQuery query,
        IRecipeRepository recipeRepository
    )
    {
        var page = query.Pagination?.Page ?? 1;
        var pageSize = query.Pagination?.PageSize ?? 20;

        if (page <= 0)
            throw new ArgumentException("Page must be greater than 0.");

        if (pageSize <= 0 || pageSize > 100)
            throw new ArgumentException("PageSize must be greater than 0 and less than or equal to 100.");

        var searchText = query.Filter?.SearchText?.Trim();
        if (string.IsNullOrWhiteSpace(searchText))
            searchText = null;

        var (minimumCookingTime, maximumCookingTime) = GetCookingTimeRange(query.Filter?.CookingTime);

        var criteria = new RecipeSearchCriteria(
            searchText,
            minimumCookingTime,
            maximumCookingTime,
            MapSortBy(query.SortBy),
            (page - 1) * pageSize,
            pageSize
        );

        var result = await recipeRepository.SearchAsync(criteria);
        var totalPages = result.TotalItems == 0
            ? 0
            : (int)Math.Ceiling(result.TotalItems / (double)pageSize);

        return new SearchRecipesResponse(
            result.Items.Select(MapSummary).ToList(),
            page,
            pageSize,
            result.TotalItems,
            totalPages
        );
    }

    private static (int? Minimum, int? Maximum) GetCookingTimeRange(RecipeCookingTimeFilter? cookingTime)
    {
        return cookingTime switch
        {
            RecipeCookingTimeFilter.Quick => (null, 30 * 60),
            RecipeCookingTimeFilter.Medium => (31 * 60, 90 * 60),
            RecipeCookingTimeFilter.Long => ((90 * 60) + 1, null),
            _ => (null, null)
        };
    }

    private static RecipeSearchSortBy MapSortBy(RecipeSortBy sortBy)
    {
        return sortBy switch
        {
            RecipeSortBy.Oldest => RecipeSearchSortBy.Oldest,
            RecipeSortBy.NameAscending => RecipeSearchSortBy.NameAscending,
            RecipeSortBy.NameDescending => RecipeSearchSortBy.NameDescending,
            _ => RecipeSearchSortBy.Newest
        };
    }

    private static RecipeSummaryDto MapSummary(Recipe recipe)
    {
        return new RecipeSummaryDto(
            recipe.Id,
            recipe.Name,
            null,
            recipe.TotalTimeInSeconds,
            recipe.CreatedAt
        );
    }
}
