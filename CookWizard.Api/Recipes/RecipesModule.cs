using Carter;
using CookWizard.Application.Features.Recipes.Commands;
using CookWizard.Application.Features.Recipes.Queries;
using Microsoft.AspNetCore.Mvc;

namespace CookWizard.Api.Recipes;

public class RecipesModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var recipesGroup = app.MapGroup("/api");

        recipesGroup.MapPost("/recipes", CreateRecipe);
        recipesGroup.MapGet("/recipes/{id}", GetRecipe);
    }

    private static async Task<IResult> CreateRecipe()
    {
        //var result = await mediator.Send(createRecipe);
        return Results.Ok();
    }

    private static async Task<IResult> GetRecipe(string id)
    {
        /*
        var query = new GetRecipeByIdQuery(id);
        var result = await mediator.Send(query);
        return Results.Ok(result);
        */
        return Results.Ok();
    }
}
