using CookWizard.Application.Common;
using CookWizard.Application.Features.Recipes.Commands.CreateRecipe;
using CookWizard.Application.Features.Recipes.Queries.GetRecipeById;
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

    [HttpPost(Name = "create")]
    public async Task<ActionResult<Guid>> Create(CreateRecipeCommand command)
    {
        var result = await _mediator.SendAsync(command);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Recipe>> GetById(Guid id)
    {
        var query = new GetRecipeByIdQuery(id);
        var result = await _mediator.SendAsync(query);

        if (result == null)
            return NotFound(new { Message = $"No se encontró la receta con ID: {id}" });

        return Ok(result);
    }

}
