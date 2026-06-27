using CookWizard.Application.Recipes.GetRecipe;
using CookWizard.Domain.Recipes.Models;
using Mapster;

namespace CookWizard.Application.Common.Mappings;

public static class RecipeMappingConfig
{
    public static void RegisterMappings()
    {
        TypeAdapterConfig<Recipe, GetRecipeResponse>
            .NewConfig()
            .Map(dest => dest.Difficulty, src => src.Difficulty.ToString());

        TypeAdapterConfig<Section, GetRecipeSectionResponse>
            .NewConfig();

        TypeAdapterConfig<Ingredient, GetRecipeIngredientResponse>
            .NewConfig()
            .Map(dest => dest.RawIngredient, src => src.Raw);

        TypeAdapterConfig<PreparationStep, GetRecipeStepsResponse>
            .NewConfig()
            .Map(dest => dest.Order, src => src.Order)
            .Map(dest => dest.Instructions, src => src.Description);
    }
}
