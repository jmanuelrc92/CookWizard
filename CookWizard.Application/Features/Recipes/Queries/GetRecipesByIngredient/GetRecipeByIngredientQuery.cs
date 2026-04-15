using CookWizard.Application.Common;
using CookWizard.Domain.Entities;
using CookWizard.Domain.Interfaces;

namespace CookWizard.Application.Features.Recipes.Queries;

public record GetRecipesByIngredientsQuery(List<string> products) : IRequestCustom<CookWizardApiResult<IEnumerable<Recipe>>>;

public class GetRecipesByIngredientsHandler
    : IRequestHandlerCustom<GetRecipesByIngredientsQuery, CookWizardApiResult<IEnumerable<Recipe>>>
{
    private readonly IRecipeRepository _repository;

    public GetRecipesByIngredientsHandler(IRecipeRepository repository)
    {
        _repository = repository;
    }

    public async Task<CookWizardApiResult<IEnumerable<Recipe>>> HandleAsync(
        GetRecipesByIngredientsQuery request,
        CancellationToken cancellationToken
    )
    {
        if (request.products == null || !request.products.Any())
            return CookWizardApiResult<IEnumerable<Recipe>>.Failure("Se necesita al menos un ingrediente");

        var foundRecipes = await _repository.SearchByIngredientAsync(request.products);

        if (!foundRecipes.Any())
            return CookWizardApiResult<IEnumerable<Recipe>>.Failure("No existen recetas con esos ingredientes");

        return CookWizardApiResult<IEnumerable<Recipe>>.Success(foundRecipes);
    }
}