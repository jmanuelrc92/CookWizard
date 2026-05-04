using CookWizard.Application.Common;
using CookWizard.Application.Common.DTOs;
using CookWizard.Domain.Entities;
using CookWizard.Domain.Interfaces.Repository;
using MapsterMapper;
using MediatR;

namespace CookWizard.Application.Features.Recipes.Queries;
public record GetRecipeByIdQuery(string Id) : IRequest<ResultObject<RecipeDTO>>;

public class GetRecipeByIdHandler : IRequestHandler<GetRecipeByIdQuery, ResultObject<RecipeDTO>>
{
    private readonly IRecipeRepository _repository;
    private readonly IMapper _mapper;

    public GetRecipeByIdHandler(IRecipeRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ResultObject<RecipeDTO>> Handle(GetRecipeByIdQuery request, CancellationToken cancellationToken = default)
    {
        var recipe = await _repository.GetByIdAsync(request.Id);
        if (recipe == null)
        {
            return ResultObject<RecipeDTO>.Failure($"La receta con ID { request.Id } no existe.");
        }
        var recipeDTO = _mapper.Map<RecipeDTO>(recipe);
        return ResultObject<RecipeDTO>.Success(recipeDTO);
    }
}
