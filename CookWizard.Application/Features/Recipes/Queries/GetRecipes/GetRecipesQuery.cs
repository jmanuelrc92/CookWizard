using CookWizard.Application.Common;
using CookWizard.Domain.Entities;
using CookWizard.Domain.Interfaces;

namespace CookWizard.Application.Features.Recipes.Queries.GetRecipes;
public record PagedResult<T>(IEnumerable<T> Items, int PageNumber, int PageSize, long TotalItems);

public record GetRecipesQuery(int PageNumber, int PageSize) : IRequestCustom<PagedResult<Recipe>>;

public class GetRecipesHandler : IRequestHandlerCustom<GetRecipesQuery, PagedResult<Recipe>>
{
    private readonly IRecipeRepository _repository;

    public GetRecipesHandler(IRecipeRepository repository) => _repository = repository;

    public async Task<PagedResult<Recipe>> HandleAsync(GetRecipesQuery request, CancellationToken ct)
    {
        // Validaciones básicas
        var page = request.PageNumber <= 0 ? 1 : request.PageNumber;
        var size = request.PageSize <= 0 ? 10 : request.PageSize;

        var (items, total) = await _repository.GetRecipes(page, size);

        return new PagedResult<Recipe>(items, page, size, total);
    }
}