using CookWizard.Application.Common.DTOs;
using CookWizard.Application.Features.Recipes.Commands;
using CookWizard.Application.Features.Recipes.Queries;
using CookWizard.Application.Features.Recipes.Queries.SearchRecipes;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CookWizard.Api.Controllers;

[Route("api/[controller]")]
public class RecipesController : ApiController
{
    private readonly IMediator _mediator;

    public RecipesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> CreateRecipe(CreateRecipeCommand command)
    {
        var result = await _mediator.Send(command);
        return HandleResult<string>(result);
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<RecipeDTO>>> GetRecipes([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var query = new GetRecipesQuery(pageNumber, pageSize);
        var result = await _mediator.Send(query);
        return HandleResult<PagedResult<RecipeDTO>>(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RecipeDTO>> GetRecipe(string id)
    {
        var query = new GetRecipeByIdQuery(id);
        var result = await _mediator.Send(query);

        return HandleResult(result);
    }

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<RecipeDTO>>> GetRecipesByIngredients([FromBody]List<string> ingredients)
    {
        var query = new GetRecipesByIngredientsQuery(ingredients);
        var result = await _mediator.Send(query);

        return HandleResult<IEnumerable<RecipeDTO>>(result);
    }

    [HttpGet("search")]
    public async Task<ActionResult<PagedResult<RecipeDTO>>> Search(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] List<string>? products = null,
        [FromQuery] string? difficulty = null,
        [FromQuery] int? maxTime = null,
        [FromQuery] int? minPortions = null
    )
    {
        var query = new SearchRecipesQuery(
            pageNumber,
            pageSize,
            products,
            difficulty,
            maxTime,
            minPortions
        );

        var result = await _mediator.Send(query);
        return HandleResult(result);
    }

}
