using CookWizard.Application.Common;
using CookWizard.Application.Features.Recipes.Commands.CreateRecipe;
using CookWizard.Application.Features.Recipes.Queries.GetRecipeById;
using CookWizard.Application.Features.Recipes.Queries.GetRecipes;
using CookWizard.Application.Features.Recipes.Queries.GetRecipesByIngredient;
using CookWizard.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CookWizard.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RecipesController : ControllerBase
{
    private readonly IMediatorCustom _mediator;

    public RecipesController(IMediatorCustom mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("create")]
    public async Task<ActionResult<Guid>> Create(CreateRecipeCommand command)
    {
        var result = await _mediator.SendAsync(command);
        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<Recipe>>> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var query = new GetRecipesQuery(pageNumber, pageSize);
        var result = await _mediator.SendAsync(query);
        return Ok(result);
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Recipe>> GetById(Guid id)
    {
        var query = new GetRecipeByIdQuery(id);
        var result = await _mediator.SendAsync(query);

        if (result == null)
            return NotFound(new { Message = $"No se encontró la receta con ID: {id}" });

        return Ok(result);
    }

    [HttpPost("searchwith")]
    public async Task<ActionResult<IEnumerable<Recipe>>> GetRecipesByIngredients([FromBody]List<string> products)
    {
        var query = new GetRecipesByIngredientsQuery(products);
        var result = await _mediator.SendAsync(query);

        return Ok(result);
    }

}
