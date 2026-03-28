using CookWizard.Application.Common;
using CookWizard.Domain.Entities;
using CookWizard.Domain.Interfaces;

namespace CookWizard.Application.Features.Recipes.Queries.GetRecipesByIngredient;

public record GetRecipesByIngredientsQuery(List<string> products) : IRequestCustom<IEnumerable<Recipe>>;

public class GetRecipesByIngredientsHandler
    : IRequestHandlerCustom<GetRecipesByIngredientsQuery, IEnumerable<Recipe>>
{
    private readonly IRecipeRepository _repository;

    public GetRecipesByIngredientsHandler(IRecipeRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Recipe>> HandleAsync(
        GetRecipesByIngredientsQuery request,
        CancellationToken cancellationToken)
    {
        if (request.products == null || !request.products.Any())
            return Enumerable.Empty<Recipe>();

        return await _repository.SearchByIngredientAsync(request.products);
    }
}