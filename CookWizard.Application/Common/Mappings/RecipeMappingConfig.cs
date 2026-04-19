using CookWizard.Application.Common.DTOs;
using CookWizard.Application.Features.Recipes.Commands;
using CookWizard.Domain.Entities;
using Mapster;

namespace CookWizard.Application.Common.Mappings;

public static class RecipeMappingConfig
{
    public static void RegisterMappings()
    {
        TypeAdapterConfig<Recipe, RecipeDTO>
            .NewConfig()
            .Map(dest => dest.Difficulty, src => src.Difficulty.ToString());

        TypeAdapterConfig<Section, SectionDTO>.NewConfig();
        TypeAdapterConfig<Ingredient, IngredientDTO>.NewConfig();
        TypeAdapterConfig<PreparationStep, PreparationStepDTO>
            .NewConfig()
            .Map(dest => dest.Description, src => src.Description);
    }
}
