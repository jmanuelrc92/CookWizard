using CookWizard.Application.Common;
using CookWizard.Application.Common.DTOs;
using CookWizard.Domain.Interfaces.Repository;
using MapsterMapper;
using MediatR;

namespace CookWizard.Application.Features.Recipes.Queries.SearchRecipes;

public record SearchRecipesQuery(
    int PageNumber,
    int PageSize,
    List<string>? Ingredients,
    string? Difficulty,
    int? MaxTimeInSeconds,
    int? MinPortions
) : IRequest<ResultObject<PagedResult<RecipeDTO>>>;


public class SearchRecipesHandler
    : IRequestHandler<SearchRecipesQuery, ResultObject<PagedResult<RecipeDTO>>>
{
    private readonly IRecipeRepository _repository;
    private readonly IMapper _mapper;

    public SearchRecipesHandler(IRecipeRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ResultObject<PagedResult<RecipeDTO>>> Handle(
        SearchRecipesQuery request,
        CancellationToken ct)
    {
        var page = request.PageNumber <= 0 ? 1 : request.PageNumber;
        var size = request.PageSize <= 0 ? 10 : request.PageSize;

        var (items, total) = await _repository.SearchAsync(
            page,
            size,
            request.Ingredients,
            request.Difficulty,
            request.MaxTimeInSeconds,
            request.MinPortions
        );

        var dtoItems = _mapper.Map<IEnumerable<RecipeDTO>>(items);

        var totalPages = (int)Math.Ceiling(total / (double)size);

        var result = new PagedResult<RecipeDTO>(
            dtoItems,
            page,
            size,
            total,
            totalPages
        );

        return ResultObject<PagedResult<RecipeDTO>>.Success(result);
    }
}