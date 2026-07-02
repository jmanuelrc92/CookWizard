using CookWizard.Application.Recipes.UseCases.GetRecipe;
using CookWizard.Domain.Recipes.Models;
using Mapster;

namespace CookWizard.Application.Recipes.Mappings;

public static class RecipeMappingConfig
{
    public static void RegisterMappings()
    {
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
