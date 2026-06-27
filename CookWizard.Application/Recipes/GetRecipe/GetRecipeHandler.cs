using CookWizard.Domain.Recipes.Repository;
using MapsterMapper;

namespace CookWizard.Application.Recipes.GetRecipe;

public class GetRecipeHandler
{
    public async Task<GetRecipeResponse> Handle(
        GetRecipeQuery query,
        IRecipeRepository recipeRepository,
        IMapper mapper
    )
    {
        var recipe = await recipeRepository.GetByIdAsync(query.Id);
        if (recipe == null)
            throw new KeyNotFoundException($"La receta con ID {query.Id} no existe.");
        return mapper.Map<GetRecipeResponse>(recipe);
    }
}
