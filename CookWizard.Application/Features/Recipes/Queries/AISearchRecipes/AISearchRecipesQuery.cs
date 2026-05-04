using CookWizard.Application.Common;
using CookWizard.Application.Common.DTOs;
using CookWizard.Application.Features.Recipes.Queries.SearchRecipes;
using CookWizard.Domain.Interfaces.Services;
using MediatR;

namespace CookWizard.Application.Features.Recipes.Queries.AISearchRecipes;

public record AISearchRecipesQuery(string Prompt, int PageNumber, int PageSize) : IRequest<ResultObject<PagedResult<RecipeDTO>>>;

public class AISearchRecipesHandler
    : IRequestHandler<AISearchRecipesQuery, ResultObject<PagedResult<RecipeDTO>>>
{
    private readonly IAIRecipeService _aiService;
    private readonly IMediator _mediator;

    public AISearchRecipesHandler(IAIRecipeService aiService, IMediator mediator)
    {
        _aiService = aiService;
        _mediator = mediator;
    }

    public async Task<ResultObject<PagedResult<RecipeDTO>>> Handle(
        AISearchRecipesQuery request,
        CancellationToken ct)
    {
        var criteria = await _aiService.ParsePrompt(request.Prompt);

        var query = new SearchRecipesQuery(
            request.PageNumber,
            request.PageSize,
            criteria.Ingredients,
            criteria.Difficulty,
            criteria.MaxTime,
            null
        );

        return await _mediator.Send(query);
    }
}