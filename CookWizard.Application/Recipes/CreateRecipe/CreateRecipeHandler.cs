using CookWizard.Domain.Recipes.Models;
using CookWizard.Domain.Recipes.Repository;

namespace CookWizard.Application.Recipes.CreateRecipe;

public class CreateRecipeHandler
{
    public async Task<CreateRecipeResponse> Handle(
        CreateRecipeCommand command,
        IRecipeRepository recipeRepository
    )
    {
        var recipe = new Recipe(
            command.Name,
            command.Portions,
            command.TotalTimeInSeconds,
            Difficulty.Easy
        );

        foreach(var section in command.Sections)
        {
            var newSection = new Section(section.Name);
            foreach(var ingredient in section.Ingredients)
            {
                //To-do
                var newIngredient = new Ingredient(
                    ingredient.Quantity,
                    ingredient.Product,
                    ingredient.Raw
                );

                if (!string.IsNullOrWhiteSpace(ingredient.Notes))
                    newIngredient.AddNotes(ingredient.Notes);

                newSection.AddIngredient(newIngredient);
            }
            foreach (var step in section.Steps)
            {
                newSection.AddStep(step.Description);
            }
            recipe.AddSection(newSection);
        }

        var recipeId = await recipeRepository.AddAsync(recipe);

        return new CreateRecipeResponse(
            recipeId,
            recipe.Name,
            recipe.Portions,
            recipe.TotalTimeInSeconds,
            recipe.CreatedAt
        );
    }
}
