using CookWizard.Application.Common;
using CookWizard.Domain.Entities;
using CookWizard.Domain.Interfaces;

namespace CookWizard.Application.Features.Recipes.Commands;

public record CreateRecipeCommand(
    string Name,
    int Portions,
    string Category,
    List<IngredientDTO> Ingredients,
    List<PreparationStepDTO> PreparationSteps,
    PreparationTimeDTO PreparationTime
) : IRequestCustom<CookWizardApiResult<Guid>>;

public record IngredientDTO(double Quantity, string Unit, string Product, string? PreparationNotes);
public record PreparationStepDTO(int Order, string Step);
public record PreparationTimeDTO(int Time, string Unit, int TimeInSeconds);

public class CreateRecipeHandler : IRequestHandlerCustom<CreateRecipeCommand, CookWizardApiResult<Guid>>
{
    private readonly IRecipeRepository _recipeRepository;
    public CreateRecipeHandler(IRecipeRepository recipeRepository)
    {
        _recipeRepository = recipeRepository;
    }

    public async Task<CookWizardApiResult<Guid>> HandleAsync(CreateRecipeCommand request, CancellationToken cancellationToken)
    {
        // --- 1. Validaciones de Negocio ---
        if (request.Portions <= 0)
            return CookWizardApiResult<Guid>.Failure("Las porciones deben ser mayores a cero.");

        if (request.PreparationTime.Time < 0)
            return CookWizardApiResult<Guid>.Failure("El tiempo de preparación no puede ser negativo.");

        if (!request.Ingredients.Any())
            return CookWizardApiResult<Guid>.Failure("La receta debe tener al menos un ingrediente.");

        // Mapping
        var newRecipe = new Recipe
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Category = request.Category,
            Portions = request.Portions,
            PreparationTime = new PreparationTime
            {
                Time = request.PreparationTime.Time,
                Unit = request.PreparationTime.Unit,
                TimeInSeconds = request.PreparationTime.TimeInSeconds
            },
            // Mapeo de listas anidadas
            Ingredients = request.Ingredients.Select(i => new Ingredient
            {
                Quantity = i.Quantity,
                Unit = i.Unit,
                Product = i.Product,
                PreparationNotes = i.PreparationNotes
            }).ToList(),
            PreparationSteps = request.PreparationSteps.Select(p => new PreparationStep
            {
                Order = p.Order,
                Step = p.Step
            }).OrderBy(p => p.Order).ToList() // Aseguramos el orden de los pasos
        };

        //Persistance
        var createdRecipe = await _recipeRepository.CreateAsync(newRecipe);
        return CookWizardApiResult<Guid>.Success(createdRecipe);
    }
}