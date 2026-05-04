using CookWizard.Application.Common;
using CookWizard.Application.Common.DTOs;
using CookWizard.Domain.Interfaces.Repository;
using MapsterMapper;
using MediatR;

namespace CookWizard.Application.Features.Recipes.Queries;

public record PagedResult<T>(
    IEnumerable<T> Items,
    int PageNumber,
    int PageSize,
    long TotalItems,
    int TotalPages
);

public record GetRecipesQuery(int PageNumber, int PageSize) : IRequest<ResultObject<PagedResult<RecipeDTO>>>;

public class GetRecipesHandler : IRequestHandler<GetRecipesQuery, ResultObject<PagedResult<RecipeDTO>>>
{
    private readonly IRecipeRepository _repository;
    private readonly IMapper _mapper;

    public GetRecipesHandler(IRecipeRepository repository, IMapper mapper)
    {
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<ResultObject<PagedResult<RecipeDTO>>> Handle(GetRecipesQuery request, CancellationToken ct)
    {
        // Validaciones básicas
        var page = request.PageNumber <= 0 ? 1 : request.PageNumber;
        var size = request.PageSize <= 0 ? 10 : request.PageSize;

        var (items, total) = await _repository.GetRecipes(page, size);

        if (!items.Any())
            return ResultObject<PagedResult<RecipeDTO>>.Failure("No existen recetas");

        var dtoItems = _mapper.Map<IEnumerable<RecipeDTO>>(items);

        return ResultObject<PagedResult<RecipeDTO>>.Success(
            new PagedResult<RecipeDTO>(dtoItems, page, size, total, 0)
        );
    }
}