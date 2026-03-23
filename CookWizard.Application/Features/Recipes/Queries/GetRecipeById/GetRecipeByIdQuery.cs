using CookWizard.Application.Common;
using CookWizard.Domain.Entities;
using CookWizard.Domain.Interfaces;

namespace CookWizard.Application.Features.Recipes.Queries.GetRecipeById;
public record GetRecipeByIdQuery(Guid Id) : IRequestCustom<Recipe>;

public class GetRecipeByIdHandler : IRequestHandlerCustom<GetRecipeByIdQuery, Recipe?>
{
    private readonly IRecipeRepository _repository;

    public GetRecipeByIdHandler(IRecipeRepository repository)
    {
        _repository = repository;
    }

    public async Task<Recipe?> HandleAsync(GetRecipeByIdQuery request, CancellationToken cancellationToken = default)
    {
        return await _repository.GetByIdAsync(request.Id);
    }
}
