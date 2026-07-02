using Carter;
using CookWizard.Application.Recipes.UseCases.CreateRecipe;
using CookWizard.Application.Recipes.UseCases.GetRecipe;
using Wolverine;

namespace CookWizard.Api.Recipes;

public class RecipesModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var recipesGroup = app.MapGroup("/api");

        recipesGroup.MapPost("/recipes", CreateRecipe)
            .RequireAuthorization();
        recipesGroup.MapGet("/recipes/{id}", GetRecipe)
            .RequireAuthorization();
    }

    private static async Task<IResult> CreateRecipe(CreateRecipeCommand command, IMessageBus bus)
    {
        var result = await bus.InvokeAsync<CreateRecipeResponse>(command);
        return Results.Ok(result);
    }

    private static async Task<IResult> GetRecipe(string id, IMessageBus bus)
    {
        var result = await bus.InvokeAsync<GetRecipeResponse>(new GetRecipeQuery(id));
        return Results.Ok(result);
    }
}
