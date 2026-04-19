using CookWizard.Application.Common;
using CookWizard.Application.Common.DTOs;
using CookWizard.Domain.Interfaces;
using MapsterMapper;
using MediatR;

namespace CookWizard.Application.Features.Recipes.Queries;

public record GetRecipesByIngredientsQuery(List<string> products) : IRequest<ResultObject<IEnumerable<RecipeDTO>>>;

public class GetRecipesByIngredientsHandler
    : IRequestHandler<GetRecipesByIngredientsQuery, ResultObject<IEnumerable<RecipeDTO>>>
{
    private readonly IRecipeRepository _repository;
    private readonly IMapper _mapper;

    public GetRecipesByIngredientsHandler(IRecipeRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ResultObject<IEnumerable<RecipeDTO>>> Handle(
        GetRecipesByIngredientsQuery request,
        CancellationToken cancellationToken
    )
    {
        if (request.products == null || !request.products.Any())
            return ResultObject<IEnumerable<RecipeDTO>>.Failure("Se necesita al menos un ingrediente");

        var foundRecipes = await _repository.SearchByIngredientAsync(request.products);

        if (!foundRecipes.Any())
            return ResultObject<IEnumerable<RecipeDTO>>.Failure("No existen recetas con esos ingredientes");

        var recipesFoundDTO = _mapper.Map<IEnumerable<RecipeDTO>>(foundRecipes);

        return ResultObject<IEnumerable<RecipeDTO>>.Success(recipesFoundDTO);
    }
}