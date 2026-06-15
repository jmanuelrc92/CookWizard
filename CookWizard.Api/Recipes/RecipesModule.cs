using Carter;
using CookWizard.Application.Recipes.CreateRecipe;
using Wolverine;

namespace CookWizard.Api.Recipes;

public class RecipesModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var recipesGroup = app.MapGroup("/api");

        recipesGroup.MapPost("/recipes", CreateRecipe);
        recipesGroup.MapGet("/recipes/{id}", GetRecipe);
    }

    private static async Task<IResult> CreateRecipe(CreateRecipeCommand command, IMessageBus bus)
    {
        var result = await bus.InvokeAsync<CreateRecipeResponse>(command);
        return Results.Ok(result);
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
