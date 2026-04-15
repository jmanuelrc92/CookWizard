using CookWizard.Application.Common;
using CookWizard.Domain.Entities;
using CookWizard.Domain.Interfaces;

namespace CookWizard.Application.Features.Recipes.Queries;
public record PagedResult<T>(IEnumerable<T> Items, int PageNumber, int PageSize, long TotalItems);

public record GetRecipesQuery(int PageNumber, int PageSize) : IRequestCustom<CookWizardApiResult<PagedResult<Recipe>>>;

public class GetRecipesHandler : IRequestHandlerCustom<GetRecipesQuery, CookWizardApiResult<PagedResult<Recipe>>>
{
    private readonly IRecipeRepository _repository;

    public GetRecipesHandler(IRecipeRepository repository) => _repository = repository;

    public async Task<CookWizardApiResult<PagedResult<Recipe>>> HandleAsync(GetRecipesQuery request, CancellationToken ct)
    {
        // Validaciones básicas
        var page = request.PageNumber <= 0 ? 1 : request.PageNumber;
        var size = request.PageSize <= 0 ? 10 : request.PageSize;

        var (items, total) = await _repository.GetRecipes(page, size);

        if (!items.Any())
            return CookWizardApiResult<PagedResult<Recipe>>.Failure("No existen recetas");

        return CookWizardApiResult<PagedResult<Recipe>>.Success(
            new PagedResult<Recipe>(items, page, size, total)
        );
    }
}