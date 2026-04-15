using CookWizard.Application.Common;
using CookWizard.Domain.Entities;
using CookWizard.Domain.Interfaces;

namespace CookWizard.Application.Features.Recipes.Queries;
public record GetRecipeByIdQuery(Guid Id) : IRequestCustom<CookWizardApiResult<Recipe>>;

public class GetRecipeByIdHandler : IRequestHandlerCustom<GetRecipeByIdQuery, CookWizardApiResult<Recipe>>
{
    private readonly IRecipeRepository _repository;

    public GetRecipeByIdHandler(IRecipeRepository repository)
    {
        _repository = repository;
    }

    public async Task<CookWizardApiResult<Recipe>> HandleAsync(GetRecipeByIdQuery request, CancellationToken cancellationToken = default)
    {
        var recipe = await _repository.GetByIdAsync(request.Id);
        if (recipe == null)
        {
            return CookWizardApiResult<Recipe>.Failure($"La receta con ID { request.Id } no existe.");
        }
        return CookWizardApiResult<Recipe>.Success(recipe);
    }
}
